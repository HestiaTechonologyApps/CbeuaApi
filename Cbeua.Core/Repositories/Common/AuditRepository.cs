using Microsoft.EntityFrameworkCore;
using System;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Linq;
using System.Reflection;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading.Tasks;
using Cbeua.Core.Helpers;
using Cbeua.Domain.DTO;
using Cbeua.InfraCore.Data;
using Cbeua.Domain.Entities.Common;

namespace Cbeua.Core.Repositories
{
    public class AuditRepository : IAuditRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly ICurrentUserService _currentUser;

        public AuditRepository(AppDbContext dbContext, ICurrentUserService currentUser)
        {
            _dbContext = dbContext;
            _currentUser = currentUser;
        }

        public async Task LogAuditAsync<T>(
            string tableName,
            string action,
            int? recordId,
            T oldEntity,
            T newEntity,
            string changedBy
        ) where T : class
        {
            string changeDetails = string.Empty;

            // Get scalar properties only (exclude navigation properties)
            var propertiesToCompare = GetScalarProperties<T>();

            // Build ChangeDetails based on Action
            switch (action.ToLower())
            {
                case "create":
                    changeDetails = BuildCreateChangeDetails(newEntity, propertiesToCompare);
                    break;

                case "update":
                    changeDetails = BuildUpdateChangeDetails(oldEntity, newEntity, propertiesToCompare);

                    // If no changes detected, skip audit logging
                    if (string.IsNullOrEmpty(changeDetails))
                    {
                        return;
                    }
                    break;

                case "delete":
                    changeDetails = BuildDeleteChangeDetails(oldEntity, propertiesToCompare);
                    break;

                default:
                    throw new ArgumentException($"Invalid action: {action}");
            }

            // Create AuditLog entity
            var auditLog = new AuditLog
            {
                LogID = Guid.NewGuid(),
                TableName = tableName.ToUpper(),
                Action = action.ToUpper (),
                RecordID = recordId,
                ChangedBy = _currentUser.Email,
                ChangedAt = DateTime.UtcNow,
                ChangeDetails = changeDetails
            };

            // Save to database
            await _dbContext.AuditLogs.AddAsync(auditLog);
            await _dbContext.SaveChangesAsync();
        }


        public async Task<List<AuditLogDTO>> GetAuditLogsForEntityAsync(string tableName, int recordId)
        {
            var auditLogs = await _dbContext.AuditLogs
                .Where(a => a.TableName.ToUpper () == tableName.ToLower() && a.RecordID == recordId)
                .OrderByDescending(a => a.ChangedAt)
                .ToListAsync();

            return ConvertToAuditLogDTOs(auditLogs);
        }

        /// <summary>
        /// Gets all audit logs for a specific table
        /// </summary>
        public async Task<List<AuditLogDTO>> GetAuditLogsForTableAsync(string tableName)
        {
            var auditLogs = await _dbContext.AuditLogs
                .Where(a => a.TableName == tableName)
                .OrderByDescending(a => a.ChangedAt)
                .ToListAsync();

            return ConvertToAuditLogDTOs(auditLogs);
        }

        #region Private Helper Methods

        /// <summary>
        /// Gets all scalar properties of type T (excludes navigation properties)
        /// </summary>
        private List<PropertyInfo> GetScalarProperties<T>() where T : class
        {
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var scalarProperties = properties
                .Where(p => !IsNavigationProperty(p))
                .ToList();

            return scalarProperties;
        }

        /// <summary>
        /// Determines if a property is a navigation property
        /// </summary>
        private bool IsNavigationProperty(PropertyInfo property)
        {
            var propertyType = property.PropertyType;

            // Check if it's a collection (ICollection, IEnumerable, List, etc.)
            if (propertyType.IsGenericType)
            {
                var genericTypeDef = propertyType.GetGenericTypeDefinition();

                if (genericTypeDef == typeof(ICollection<>) ||
                    genericTypeDef == typeof(IEnumerable<>) ||
                    genericTypeDef == typeof(List<>) ||
                    genericTypeDef == typeof(IList<>))
                {
                    return true;
                }
            }

            // Check if it's a reference type (but allow string and nullable value types)
            if (propertyType.IsClass &&
                propertyType != typeof(string) &&
                !propertyType.IsValueType)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Builds ChangeDetails JSON for CREATE operation
        /// </summary>
        private string BuildCreateChangeDetails<T>(T newEntity, List<PropertyInfo> properties) where T : class
        {
            var newRecord = new Dictionary<string, object>();

            foreach (var property in properties)
            {
                var value = property.GetValue(newEntity);

                newRecord[property.Name] = new
                {
                    Value = value?.ToString() ?? "NULL",
                    Remark = "Added"
                };
            }

            var changeObject = new
            {
                Operation = "Create",
                NewRecord = newRecord
            };

            return JsonSerializer.Serialize(changeObject, new JsonSerializerOptions
            {
                WriteIndented = true
            });
        }

        /// <summary>
        /// Builds ChangeDetails JSON for UPDATE operation
        /// </summary>
        private string BuildUpdateChangeDetails<T>(T oldEntity, T newEntity, List<PropertyInfo> properties) where T : class
        {
            var changes = new List<object>();

            foreach (var property in properties)
            {
                var oldValue = property.GetValue(oldEntity);
                var newValue = property.GetValue(newEntity);

                // Compare values
                if (!AreValuesEqual(oldValue, newValue))
                {
                    changes.Add(new
                    {
                        PropertyName = property.Name,
                        OldValue = oldValue?.ToString() ?? "NULL",
                        NewValue = newValue?.ToString() ?? "NULL",
                        Remark = "Updated"
                    });
                }
            }

            // If no changes detected, return empty string
            if (changes.Count == 0)
            {
                return string.Empty;
            }

            var changeObject = new
            {
                Operation = "Update",
                Changes = changes
            };

            return JsonSerializer.Serialize(changeObject, new JsonSerializerOptions
            {
                WriteIndented = true
            });
        }

        /// <summary>
        /// Builds ChangeDetails JSON for DELETE operation
        /// </summary>
        private string BuildDeleteChangeDetails<T>(T oldEntity, List<PropertyInfo> properties) where T : class
        {
            var deletedRecord = new Dictionary<string, object>();

            foreach (var property in properties)
            {
                var value = property.GetValue(oldEntity);

                deletedRecord[property.Name] = new
                {
                    Value = value?.ToString() ?? "NULL",
                    Remark = "Removed"
                };
            }

            var changeObject = new
            {
                Operation = "Delete",
                DeletedRecord = deletedRecord
            };

            return JsonSerializer.Serialize(changeObject, new JsonSerializerOptions
            {
                WriteIndented = true
            });
        }

        /// <summary>
        /// Compares two values for equality
        /// </summary>
        private bool AreValuesEqual(object oldValue, object newValue)
        {
            // Both null
            if (oldValue == null && newValue == null)
                return true;

            // One is null
            if (oldValue == null || newValue == null)
                return false;

            // Compare values
            return oldValue.Equals(newValue);
        }
      

        /// <summary>
        /// Converts AuditLog entities to AuditLogDTOs with parsed ChangeDetails
        /// </summary>
        private List<AuditLogDTO> ConvertToAuditLogDTOs(List<AuditLog> auditLogs)
        {
            var auditLogDTOs = new List<AuditLogDTO>();

            foreach (var auditLog in auditLogs)
            {
                var auditLogDTO = new AuditLogDTO
                {
                    LogID = auditLog.LogID,
                    TableName = auditLog.TableName,
                    Action = auditLog.Action,
                    RecordID = auditLog.RecordID,
                    ChangedBy = auditLog.ChangedBy,
                    ChangedAt = CustomDateHelper.ConvertToLocalTimeFormat(auditLog.ChangedAt) ,
                    ChangeDetails = "",// no need to show json string
                    Changes = ParseChangeDetails(auditLog.ChangeDetails, auditLog.Action)
                };

                auditLogDTOs.Add(auditLogDTO);
            }

            return auditLogDTOs;
        }

        /// <summary>
        /// Parses JSON ChangeDetails and converts to List of AuditLogChangeDetailsDTO
        /// </summary>
        private List<AuditLogChangeDetailsDTO> ParseChangeDetails(string changeDetailsJson, string action)
        {
            var changes = new List<AuditLogChangeDetailsDTO>();

            if (string.IsNullOrWhiteSpace(changeDetailsJson))
            {
                return changes;
            }

            try
            {
                using (JsonDocument document = JsonDocument.Parse(changeDetailsJson))
                {
                    var root = document.RootElement;

                    switch (action.ToLower())
                    {
                        case "create":
                            changes = ParseCreateOperation(root);
                            break;

                        case "update":
                            changes = ParseUpdateOperation(root);
                            break;

                        case "delete":
                            changes = ParseDeleteOperation(root);
                            break;

                        case "softdelete":
                            changes = ParseSoftDeleteOperation(root);
                            break;

                        case "restore":
                            changes = ParseRestoreOperation(root);
                            break;

                        default:
                            // Unknown action, return empty list
                            break;
                    }
                }
            }
            catch (JsonException ex)
            {
                // Log exception if needed
                // Return empty list on parse error
            }

            return changes;
        }
        #endregion
        #region Parse Operations

        /// <summary>
        /// Parses CREATE operation JSON
        /// Format: { "Operation": "Create", "NewRecord": { "PropertyName": { "Value": "...", "Remark": "Added" }, ... } }
        /// </summary>
        private List<AuditLogChangeDetailsDTO> ParseCreateOperation(JsonElement root)
        {
            var changes = new List<AuditLogChangeDetailsDTO>();

            if (root.TryGetProperty("NewRecord", out JsonElement newRecord))
            {
                foreach (JsonProperty property in newRecord.EnumerateObject())
                {
                    string propertyName = property.Name;
                    string newValue = "";

                    if (property.Value.TryGetProperty("Value", out JsonElement valueElement))
                    {
                        newValue = valueElement.ToString();
                    }

                    changes.Add(new AuditLogChangeDetailsDTO
                    {
                        Item = propertyName.ToUpper(),
                        From = "N/A",
                        TO = newValue
                    });
                }
            }

            return changes;
        }

        /// <summary>
        /// Parses UPDATE operation JSON
        /// Format: { "Operation": "Update", "Changes": [ { "PropertyName": "...", "OldValue": "...", "NewValue": "...", "Remark": "Updated" }, ... ] }
        /// </summary>
        private List<AuditLogChangeDetailsDTO> ParseUpdateOperation(JsonElement root)
        {

            var ExceptionProperties = new List<string>()
            {
                "ModifiedAt",
                "ModifiedBy",
                "UpdatedAt",
                "UpdatedBy",
                "PasswordHash",
            };
            var changes = new List<AuditLogChangeDetailsDTO>();

            if (root.TryGetProperty("Changes", out JsonElement changesArray))
            {
                foreach (JsonElement change in changesArray.EnumerateArray())
                {


                    string propertyName = "";
                    string oldValue = "";
                    string newValue = "";

                    if (change.TryGetProperty("PropertyName", out JsonElement propName))
                    {
                        propertyName = propName.GetString() ?? "";
                    }

                    if (change.TryGetProperty("OldValue", out JsonElement oldVal))
                    {
                        oldValue = oldVal.GetString() ?? "";
                    }

                    if (change.TryGetProperty("NewValue", out JsonElement newVal))
                    {
                        newValue = newVal.GetString() ?? "";
                    }
                    if (!ExceptionProperties.Contains(propertyName))
                    {
                        changes.Add(new AuditLogChangeDetailsDTO
                        {
                            Item = propertyName.ToUpper(),
                            From = oldValue,
                            TO = newValue
                        });
                    }
                }
            }

            return changes;
        }

        /// <summary>
        /// Parses DELETE operation JSON
        /// Format: { "Operation": "Delete", "DeletedRecord": { "PropertyName": { "Value": "...", "Remark": "Removed" }, ... } }
        /// </summary>
        private List<AuditLogChangeDetailsDTO> ParseDeleteOperation(JsonElement root)
        {
            var changes = new List<AuditLogChangeDetailsDTO>();

            if (root.TryGetProperty("DeletedRecord", out JsonElement deletedRecord))
            {
                foreach (JsonProperty property in deletedRecord.EnumerateObject())
                {
                    string propertyName = property.Name;
                    string oldValue = "";

                    if (property.Value.TryGetProperty("Value", out JsonElement valueElement))
                    {
                        oldValue = valueElement.ToString();
                    }

                    changes.Add(new AuditLogChangeDetailsDTO
                    {
                        Item = propertyName.ToUpper(),
                        From = oldValue,
                        TO = "N/A"
                    });
                }
            }

            return changes;
        }

        /// <summary>
        /// Parses SOFT DELETE operation JSON
        /// Format: { "Operation": "SoftDelete", "Change": { "PropertyName": "IsDeleted", "OldValue": "False", "NewValue": "True", "Remark": "..." }, "RecordSnapshot": {...} }
        /// </summary>
        private List<AuditLogChangeDetailsDTO> ParseSoftDeleteOperation(JsonElement root)
        {
            var changes = new List<AuditLogChangeDetailsDTO>();

            if (root.TryGetProperty("Change", out JsonElement change))
            {
                string propertyName = "";
                string oldValue = "";
                string newValue = "";

                if (change.TryGetProperty("PropertyName", out JsonElement propName))
                {
                    propertyName = propName.GetString() ?? "";
                }

                if (change.TryGetProperty("OldValue", out JsonElement oldVal))
                {
                    oldValue = oldVal.GetString() ?? "";
                }

                if (change.TryGetProperty("NewValue", out JsonElement newVal))
                {
                    newValue = newVal.GetString() ?? "";
                }

                changes.Add(new AuditLogChangeDetailsDTO
                {
                    Item = propertyName.ToUpper(),
                    From = oldValue,
                    TO = newValue
                });
            }

            return changes;
        }

        /// <summary>
        /// Parses RESTORE operation JSON
        /// Format: { "Operation": "Restore", "Change": { "PropertyName": "IsDeleted", "OldValue": "True", "NewValue": "False", "Remark": "..." }, "RecordSnapshot": {...} }
        /// </summary>
        private List<AuditLogChangeDetailsDTO> ParseRestoreOperation(JsonElement root)
        {
            var changes = new List<AuditLogChangeDetailsDTO>();

            if (root.TryGetProperty("Change", out JsonElement change))
            {
                string propertyName = "";
                string oldValue = "";
                string newValue = "";

                if (change.TryGetProperty("PropertyName", out JsonElement propName))
                {
                    propertyName = propName.GetString() ?? "";
                }

                if (change.TryGetProperty("OldValue", out JsonElement oldVal))
                {
                    oldValue = oldVal.GetString() ?? "";
                }

                if (change.TryGetProperty("NewValue", out JsonElement newVal))
                {
                    newValue = newVal.GetString() ?? "";
                }

                changes.Add(new AuditLogChangeDetailsDTO
                {
                    Item = propertyName.ToUpper(),
                    From = oldValue,
                    TO = newValue
                });
            }

            return changes;
        }

        #endregion

    }
}
