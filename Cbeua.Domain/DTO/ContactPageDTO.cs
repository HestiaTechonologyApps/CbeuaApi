using System;

namespace Cbeua.Domain.DTO
{
    public class ContactMessageDTO
    {
        public int ContactMessageId { get; set; }
        public string FullName { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string EmailAddress { get; set; } = "";
        public string Subject { get; set; } = "";
        public string Message { get; set; } = "";
        public DateTime SubmittedAt { get; set; }
        public bool IsRead { get; set; }
        public bool IsReplied { get; set; }
        public string? AdminNotes { get; set; }
        public DateTime? RepliedAt { get; set; }
        public string? IPAddress { get; set; }
    }

    // For public submission (without authentication)
    public class ContactFormSubmissionDTO
    {
        public string FullName { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string EmailAddress { get; set; } = "";
        public string Subject { get; set; } = "";
        public string Message { get; set; } = "";
    }
}