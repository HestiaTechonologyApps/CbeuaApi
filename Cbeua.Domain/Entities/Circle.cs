using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cbeua.Domain.Entities
{
    public partial class Circle
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CircleId { get; set; }

        public int? CircleCode { get; set; }
        public string Name { get; set; } = "";
        public string Abbreviation { get; set; } = "";
        public bool IsActive { get; set; }
        public int? StateId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }    

    }
}


