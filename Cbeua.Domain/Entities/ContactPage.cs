using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cbeua.Domain.Entities
{
    public class ContactMessage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ContactMessageId { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = "";

        [MaxLength(20)]
        public string PhoneNumber { get; set; } = "";

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string EmailAddress { get; set; } = "";

        [Required]
        [MaxLength(200)]
        public string Subject { get; set; } = "";

        [Required]
        public string Message { get; set; } = "";

        public DateTime SubmittedAt { get; set; } = DateTime.Now;

        public bool IsRead { get; set; } = false;

        public bool IsReplied { get; set; } = false;

        public string? AdminNotes { get; set; }

        public DateTime? RepliedAt { get; set; }

        public string? IPAddress { get; set; }
    }
}