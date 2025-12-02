using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.Entities
{
    public class ManagingComitee
    {
        public int ManagingComiteeId { get; set; }

        public string ManagingComitteeName { get; set; } = "";

        public string Position { get; set; } = "";

        public string Description1 { get; set; } = "";


        public string Description2 { get; set; } = "";

        public string imageLocation { get; set; } = "";
        public int CompanyId { get; set; } = 0;
        public int order { get; set; }
    }
}
