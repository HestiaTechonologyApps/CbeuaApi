using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.Entities
{
    public partial class ReportType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReportTypeId { get; set; }

        public string ReportTypeName { get; set; } = "";

        public string? Description { get; set; }

        public bool IsActive { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}