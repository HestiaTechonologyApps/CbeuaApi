using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.Entities
{
    public class ManagingComitee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
