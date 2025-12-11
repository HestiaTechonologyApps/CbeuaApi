using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cbeua.Domain.Entities
{
    public partial class Month
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MonthCode { get; set; }
        public string MonthName { get; set; } = "";
        public string Abbrivation { get; set; } = "";

    }
}


