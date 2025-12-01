using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.DTO
{
    public class CommentDTO
    {
        public int CommentId { get; set; } // Primary key


        public string Description { get; set; } = "";

        public string TableName { get; set; } = "";
        public int? RecordID { get; set; }

        public int ParentCommentId { get; set; }

        public bool IsInternal { get; set; } = false;


        public DateTime? CreatedOn { get; set; }
        public String CreatedOnString { get; set; } = "";
        public String CreatedBy { get; set; } = "";


        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedOn { get; set; }
        public string DeleteOnString { get; set; } = "";
        public String DeletedBy { get; set; } = "";

    }
}
