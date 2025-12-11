using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cbeua.Domain.Entities
{
    public class SupportTicket
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SupportTicketId { get; set; }

        public String SupportTicketNum { get; set; } = "";

        public String Description { get; set; } = "";

        public String Priority { get; set; } = "";

        public String Duration { get; set; } = "";

        public String DeveloperRemark { get; set; } = "";

        public Boolean isApproved { get; set; }

        public int? ApprovedByUserId { get; set; }
        public DateTime? ApprovedDate { get; set; }


       




        
    }
}


