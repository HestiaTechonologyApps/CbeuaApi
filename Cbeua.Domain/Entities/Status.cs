using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cbeua.Domain.Entities
{
    public partial class Status
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public string Description { get; set; }
        public int? GroupId { get; set; }



    }
}


