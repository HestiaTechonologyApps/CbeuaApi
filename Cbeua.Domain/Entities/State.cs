using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cbeua.Domain.Entities
{
    public partial class State
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StateId { get; set; }
        public string Name { get; set; } = "";
        public string Abbreviation { get; set; } = "";
        public bool IsActive { get; set; }

        
    }
}


