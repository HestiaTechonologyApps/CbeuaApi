using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.DTO
{
    public class StatusDTO
    {
        public int StatusId { get; set; }
        public string Name { get; set; } = "";
        public string Abbreviation { get; set; } = "";
        public string Description { get; set; } = "";
        public int? GroupId { get; set; }
    }
}
