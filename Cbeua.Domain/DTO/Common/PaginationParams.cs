using System;

namespace Cbeua.Domain.DTO
{
    /// <summary>
    /// Base pagination parameters that can be used for any entity
    /// </summary>
    public class BasePaginationParams
    {
        private const int MaxPageSize = 100;
        private int _pageSize = 10;

        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public string? SearchTerm { get; set; }
        public string? SortBy { get; set; }
        public bool SortDescending { get; set; } = false;
    }

    public class LookupPaginationParams : BasePaginationParams
    {
        public string EntityName { get; set; } = "";
        public int LookupMasterId { get; set; }  
        public int? SelectedId { get; set; }
    }

    /// <summary>
    /// Pagination parameters for Designation entity
    /// </summary>
    public class DesignationPaginationParams : BasePaginationParams
    {
        // Optional filters specific to Designation
        public int? DesignationId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }

    /// <summary>
    /// Pagination parameters for Member entity
    /// </summary>
    public class MemberPaginationParams : BasePaginationParams
    {
        public int? BranchId { get; set; }
        public int? CategoryId { get; set; }
        public int? DesignationId { get; set; }
        public int? StatusId { get; set; }
        public int? GenderId { get; set; }
    }
}