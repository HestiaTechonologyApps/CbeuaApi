using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cbeua.Domain.Entities.Common
{
    public class Attachment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AttachmentId { get; set; } // Primary key

        public string AttachmentType { get; set; }
        public string AttachmentPath { get; set; }
        public string Description { get; set; } = "";
        public string FilePath { get; set; } = "";
        public string FileSize { get; set; } = "";
        public string FileName { get; set; } = "";
        public string FileType { get; set; } = "";
        public string TableName { get; set; } = "";
        public int? RecordID { get; set; }



        public DateTime? UploaddedOn { get; set; }
        public string UploadedBy { get; set; } = "";


        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedOn { get; set; }
        public string DeletedBy { get; set; } = "";

    }
}
