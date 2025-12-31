using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.DTO
{
    public class IndividualContrbutionDTO
    {
        public int MemeberId { get; set; }
        public int StaffNo { get; set; }
        public int MonthCode { get; set; }
        public int MonthName { get; set; }
        public int YearNo { get; set; }
        public Decimal Amount { get; set; }
    }
}
