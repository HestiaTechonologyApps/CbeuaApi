using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.DTO
{
    public class MonthDTO
    {
        public int MonthCode { get; set; }
        public string MonthName { get; set; } = "";
        public string Abbrivation { get; set; } = "";
    }
}
