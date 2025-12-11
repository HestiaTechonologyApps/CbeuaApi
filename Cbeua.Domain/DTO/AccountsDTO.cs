using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.DTO
{
    public class AccountsDTO
    {
        public long AccountId { get; set; }
        public int CircleId { get; set; }
        public int BranchId { get; set; }
        public int MemeberId { get; set; }
        public int MonthCode { get; set; }
        public int YearOf { get; set; }
        public decimal Amount { get; set; }
        public int TransMode { get; set; }
        public string Reference { get; set; } = "";
        public string Remark { get; set; } = "";
    }
}
