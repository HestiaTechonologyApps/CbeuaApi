using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cbeua.Domain.Entities
{
    public partial class Branch
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BranchId { get; set; }

        public int DpCode { get; set; }
        public string Name { get; set; } = "";
        public string Address1 { get; set; } = "";
        public string Address2 { get; set; } = "";
        public string Address3 { get; set; } = "";
        public string District { get; set; } = "";

        public string Status { get; set; } = "";
        public int CircleId { get; set; }
        public int? StateId { get; set; }


        public bool IsRegCompleted { get; set; }

       






    }
}


