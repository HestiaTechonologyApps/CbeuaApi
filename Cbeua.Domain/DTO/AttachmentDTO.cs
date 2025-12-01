namespace Cbeua.Domain.DTO
{
    public class AttachmentDTO
    {
       
        public int AttachmentId { get; set; } // Primary key

        public String AttachmentType { get; set; }
        public String AttachmentPath { get; set; }
        public string Description { get; set; } = "";

        public String FilePath { get; set; } = "";
        public String FileName { get; set; } = "";
        public String FileType { get; set; } = "";
        public string TableName { get; set; } = "";
        public int? RecordID { get; set; }

        
    
        public DateTime? UploaddedOn { get; set; }
        public String UploadedBy { get; set; } = "";


        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedOn { get; set; }
        public String DeletedBy { get; set; } = "";

    }
}
