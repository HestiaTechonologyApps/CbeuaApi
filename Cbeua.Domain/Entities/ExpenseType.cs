using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cbeua.Domain.Entities
{
    public partial class ExpenseType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExpenseTypeId { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public bool IsDeleted { get; set; } = false;
    }
}
