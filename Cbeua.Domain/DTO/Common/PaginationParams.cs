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