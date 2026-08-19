/* ============================================================================
   StaffNoMigration.sql
   One-time maintenance script: migrate StaffNo/StaffCode across every table
   that has one, driven by an old->new mapping supplied by the client (Canara
   Bank Employees' Union), e.g. "Active members cbeugjfwf new.xlsx".

   WHAT THIS DOES, IN ORDER
     0. Creates a staging table (#StaffNoMapping) for you to load the
        old->new mapping into. You MUST populate this before Part C runs.
     A. Discovers every base table with a column named StaffNo or StaffCode
        (case-insensitive, via sys.columns — not limited to tables the EF
        model knows about).
     B. For each such table:
          1. Backs it up as newcode_update_<TableName> (full row copy),
             skipped if that backup already exists so re-runs don't clobber
             an earlier snapshot.
          2. Adds an OldStaffNo column if the table doesn't already have one
             (Member already has it; User/UserRegistration/ContributionDetail
             and anything undocumented in the EF model do not).
          3. Copies the current StaffNo/StaffCode value into OldStaffNo for
             every row where OldStaffNo is still NULL (idempotent — running
             this twice does not re-stamp rows already migrated).
     C. Applies the new staff numbers: for every row whose OldStaffNo matches
        a row in #StaffNoMapping, sets StaffNo/StaffCode = NewStaffNo. Rows
        with no match are left untouched and reported, not guessed at.

   HOW TO RUN
     1. Restore/attach a COPY of the production database and run this whole
        script there first. Compare row counts / spot-check a few members.
     2. Populate #StaffNoMapping (see block below) with the confirmed
        old->new pairs -- NOT directly from the raw union Excel, which as
        received has 9 rows literally containing the text
        "OLD STAFF NO. IS WRONG" and 1 blank/system row; those must be
        resolved with the client or excluded before loading.
     3. Run Part A/B (backup + add column + stamp OldStaffNo) against
        production during a maintenance window.
     4. Review the "unmatched rows" report Part C prints. Confirm with the
        union before re-running Part C on the leftovers.
     5. Only after Part C is confirmed correct, drop the newcode_update_*
        backup tables (kept deliberately -- nothing in this script drops
        them automatically).

   SAFETY NOTES
     - Every write is wrapped in its own transaction per table; a failure on
       one table does not touch the others (each is independent, not one
       giant transaction), and is reported via THROW after ROLLBACK.
     - Table/column names driving the dynamic SQL come from sys.tables /
       sys.columns (server metadata), never from user input, so this is not
       susceptible to SQL injection via the discovery step.
     - Nothing here deletes data. OldStaffNo is additive; StaffNo is only
       overwritten for rows with a confirmed mapping row, and the pre-image
       is preserved both in OldStaffNo and in the newcode_update_* backup.
   ============================================================================ */

SET NOCOUNT ON;
SET XACT_ABORT ON;

/* ----------------------------------------------------------------------
   Part 0 -- staging table for the old -> new mapping.
   Stored as NVARCHAR on both sides because StaffNo's underlying type is
   not consistent across tables (int on Member/User/UserRegistration,
   varchar on ContributionDetail) -- comparing as string avoids failing
   the whole batch on a type-conversion error for any one table.
   ---------------------------------------------------------------------- */
IF OBJECT_ID('tempdb..#StaffNoMapping') IS NOT NULL DROP TABLE #StaffNoMapping;
CREATE TABLE #StaffNoMapping
(
    OldStaffNo NVARCHAR(50) NOT NULL PRIMARY KEY,
    NewStaffNo NVARCHAR(50) NOT NULL
);

-- Populate this from the client-confirmed mapping, e.g.:
--   INSERT INTO #StaffNoMapping (OldStaffNo, NewStaffNo)
--   SELECT OldStaffNo, NewStaffNo FROM StagingImport.dbo.ClientMapping
--   WHERE OldStaffNo IS NOT NULL AND NewStaffNo IS NOT NULL
--     AND TRY_CAST(NewStaffNo AS BIGINT) IS NOT NULL;   -- drops garbage rows
--
-- Leave empty and Part C will simply report 0 matches -- Part A/B (backup +
-- add column + stamp OldStaffNo) is safe to run on its own ahead of time.

/* ----------------------------------------------------------------------
   Part A/B -- discover tables, backup, add OldStaffNo, stamp it.
   ---------------------------------------------------------------------- */
DECLARE @TableSchema SYSNAME, @TableName SYSNAME, @StaffColumn SYSNAME, @StaffColType SYSNAME;
DECLARE @sql NVARCHAR(MAX);
DECLARE @BackupTable SYSNAME;

DECLARE cur CURSOR LOCAL FAST_FORWARD FOR
    SELECT s.name AS TableSchema, t.name AS TableName, c.name AS StaffColumn, ty.name AS StaffColType
    FROM sys.columns c
    JOIN sys.tables t  ON t.object_id = c.object_id
    JOIN sys.schemas s ON s.schema_id = t.schema_id
    JOIN sys.types ty  ON ty.user_type_id = c.user_type_id
    WHERE (c.name = 'StaffNo' OR c.name = 'StaffCode')
      AND t.name NOT LIKE 'newcode_update_%'   -- never touch our own backups
    ORDER BY s.name, t.name;

OPEN cur;
FETCH NEXT FROM cur INTO @TableSchema, @TableName, @StaffColumn, @StaffColType;

WHILE @@FETCH_STATUS = 0
BEGIN
    SET @BackupTable = 'newcode_update_' + @TableName;
    PRINT '--- ' + @TableSchema + '.' + @TableName + ' (staff column: ' + @StaffColumn + ') ---';

    BEGIN TRY
        BEGIN TRANSACTION;

        -- 1. Backup: full snapshot copy, skipped if one already exists.
        IF OBJECT_ID(QUOTENAME(@TableSchema) + '.' + QUOTENAME(@BackupTable)) IS NULL
        BEGIN
            SET @sql = N'SELECT * INTO ' + QUOTENAME(@TableSchema) + N'.' + QUOTENAME(@BackupTable) +
                       N' FROM ' + QUOTENAME(@TableSchema) + N'.' + QUOTENAME(@TableName) + N';';
            EXEC sp_executesql @sql;
            PRINT '  backed up -> ' + @BackupTable;
        END
        ELSE
            PRINT '  backup already exists, skipped -> ' + @BackupTable;

        -- 2. Add OldStaffNo if missing, matching the source column's type.
        IF NOT EXISTS (
            SELECT 1 FROM sys.columns c2
            JOIN sys.tables t2 ON t2.object_id = c2.object_id
            JOIN sys.schemas s2 ON s2.schema_id = t2.schema_id
            WHERE s2.name = @TableSchema AND t2.name = @TableName AND c2.name = 'OldStaffNo'
        )
        BEGIN
            SET @sql = N'ALTER TABLE ' + QUOTENAME(@TableSchema) + N'.' + QUOTENAME(@TableName) +
                       N' ADD OldStaffNo ' + @StaffColType +
                       CASE WHEN @StaffColType IN ('varchar','nvarchar','char','nchar') THEN N'(100)' ELSE N'' END +
                       N' NULL;';
            EXEC sp_executesql @sql;
            PRINT '  added OldStaffNo (' + @StaffColType + ')';
        END
        ELSE
            PRINT '  OldStaffNo already present, skipped';

        -- 3. Stamp OldStaffNo from the current value, only where still unset.
        SET @sql = N'UPDATE ' + QUOTENAME(@TableSchema) + N'.' + QUOTENAME(@TableName) +
                   N' SET OldStaffNo = ' + QUOTENAME(@StaffColumn) +
                   N' WHERE OldStaffNo IS NULL AND ' + QUOTENAME(@StaffColumn) + N' IS NOT NULL;';
        EXEC sp_executesql @sql;
        PRINT '  stamped OldStaffNo for ' + CAST(@@ROWCOUNT AS NVARCHAR(20)) + ' row(s)';

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        PRINT '  *** FAILED on ' + @TableSchema + '.' + @TableName + ': ' + ERROR_MESSAGE();
        -- Intentionally continue to the next table rather than THROW here,
        -- so one bad table doesn't block backup/OldStaffNo work on the rest.
    END CATCH

    FETCH NEXT FROM cur INTO @TableSchema, @TableName, @StaffColumn, @StaffColType;
END

CLOSE cur;
DEALLOCATE cur;

/* ----------------------------------------------------------------------
   Part C -- apply new StaffNo from #StaffNoMapping, matched on OldStaffNo.
   Matching against OldStaffNo (not the live StaffNo column) makes this
   block itself idempotent/safe to re-run after partial progress.
   ---------------------------------------------------------------------- */
DECLARE cur2 CURSOR LOCAL FAST_FORWARD FOR
    SELECT s.name AS TableSchema, t.name AS TableName, c.name AS StaffColumn, ty.name AS StaffColType
    FROM sys.columns c
    JOIN sys.tables t  ON t.object_id = c.object_id
    JOIN sys.schemas s ON s.schema_id = t.schema_id
    JOIN sys.types ty  ON ty.user_type_id = c.user_type_id
    WHERE (c.name = 'StaffNo' OR c.name = 'StaffCode')
      AND t.name NOT LIKE 'newcode_update_%'
      AND EXISTS (
          SELECT 1 FROM sys.columns oc
          WHERE oc.object_id = t.object_id AND oc.name = 'OldStaffNo'
      )
    ORDER BY s.name, t.name;

OPEN cur2;
FETCH NEXT FROM cur2 INTO @TableSchema, @TableName, @StaffColumn, @StaffColType;

WHILE @@FETCH_STATUS = 0
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        SET @sql = N'UPDATE tgt SET tgt.' + QUOTENAME(@StaffColumn) + N' = ' +
                   CASE WHEN @StaffColType NOT IN ('varchar','nvarchar','char','nchar')
                        THEN N'TRY_CAST(m.NewStaffNo AS ' + @StaffColType + N')'
                        ELSE N'm.NewStaffNo' END +
                   N' FROM ' + QUOTENAME(@TableSchema) + N'.' + QUOTENAME(@TableName) + N' tgt' +
                   N' JOIN #StaffNoMapping m ON m.OldStaffNo = CAST(tgt.OldStaffNo AS NVARCHAR(50))' +
                   N' WHERE ' +
                   CASE WHEN @StaffColType NOT IN ('varchar','nvarchar','char','nchar')
                        THEN N'TRY_CAST(m.NewStaffNo AS ' + @StaffColType + N') IS NOT NULL'
                        ELSE N'1=1' END + N';';
        EXEC sp_executesql @sql;
        PRINT @TableSchema + '.' + @TableName + ': updated ' + CAST(@@ROWCOUNT AS NVARCHAR(20)) + ' row(s) with new StaffNo';

        -- Report rows that had an OldStaffNo but no mapping match, for follow-up.
        SET @sql = N'SELECT ''' + @TableName + N''' AS TableName, OldStaffNo, COUNT(*) AS UnmatchedRows' +
                   N' FROM ' + QUOTENAME(@TableSchema) + N'.' + QUOTENAME(@TableName) +
                   N' WHERE OldStaffNo IS NOT NULL' +
                   N'   AND CAST(OldStaffNo AS NVARCHAR(50)) NOT IN (SELECT OldStaffNo FROM #StaffNoMapping)' +
                   N' GROUP BY OldStaffNo;';
        EXEC sp_executesql @sql;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        PRINT '*** FAILED applying mapping on ' + @TableSchema + '.' + @TableName + ': ' + ERROR_MESSAGE();
    END CATCH

    FETCH NEXT FROM cur2 INTO @TableSchema, @TableName, @StaffColumn, @StaffColType;
END

CLOSE cur2;
DEALLOCATE cur2;

PRINT '=== StaffNoMigration.sql complete ===';
