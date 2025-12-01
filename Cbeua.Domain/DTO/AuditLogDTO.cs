namespace Cbeua.Domain.DTO
{
    public class AuditLogDTO
    {
        public AuditLogDTO() { 
        this.Changes = new List<AuditLogChangeDetailsDTO>();
        }

        public Guid LogID { get; set; }
        public string TableName { get; set; }
        public string Action { get; set; }
        public int? RecordID { get; set; }

        public String? ChangedBy { get; set; }
        public string ChangedAt { get; set; }
        public string ChangeDetails { get; set; }

        public List<AuditLogChangeDetailsDTO> Changes { get; set; }
    }
    public class AuditLogChangeDetailsDTO
    {

        public string Item { get; set; }
        public string From { get; set; }
        public string TO { get; set; }

    }
}
