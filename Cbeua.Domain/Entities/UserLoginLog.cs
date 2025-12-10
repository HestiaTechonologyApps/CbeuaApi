using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Cbeua.Domain.Entities
{
    public class UserLoginLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserLoginLogId { get; set; }

      
        public int UserId { get; set; }
        public DateTime ActionTime { get; set; }

        public string ActionType { get; set; } = "Login";

    }
}
