using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.DTO
{
    public class ClaimsSettledStatsDTO
    {
        public int TotalClaimsSettled { get; set; }
        public decimal TotalAmountDisbursed { get; set; }
        public int ActiveMembers { get; set; }
    }
}
