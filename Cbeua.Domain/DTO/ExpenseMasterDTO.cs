using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.DTO
{
    public class ExpenseMasterDTO
    {
        public int ExpenseMasterId { get; set; }
        public int ExpenseTypeId { get; set; }
        public string ExpenseTypeName { get; set; } = "";
        public DateTime ExpenseDate { get; set; }
        public decimal Amount { get; set; }
        public string PaidTo { get; set; } = "";
        public string ReferenceNo { get; set; } = "";
        public string PaymentMode { get; set; } = "";
        public string Description { get; set; } = "";
        public bool IsDeleted { get; set; } = false;
        public List<AuditLogDTO> AuditLogs { get; set; } = new List<AuditLogDTO>();
    }

    public class ExpenseMasterPaginationParams
    {
        public int? ExpenseMasterId { get; set; }
        public int? ExpenseTypeId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string SearchTerm { get; set; } = "";
        public string SortBy { get; set; } = "";
        public bool SortDescending { get; set; } = false;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public bool GetAll { get; set; } = false;
    }
}
