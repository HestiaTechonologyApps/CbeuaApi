using System.ComponentModel.DataAnnotations.Schema;

namespace Cbeua.Domain.Entities
{
    public class SupportTicket
    {
        public int Id { get; set; }

        public String SupportTicketNum { get; set; }

        public String Description { get; set; }

        public String Priority { get; set; }

        public String Duration { get; set; }

        public String DeveloperRemark { get; set; }

        public Boolean isApproved { get; set; }

        public int? ApprovedByUserId { get; set; }
        public DateTime? ApprovedDate { get; set; }
        }

        //public partial class User
        //{
        //    public User()
        //    {
        //        AccountsCreatedByUser = new HashSet<Accounts>();
        //        AccountsModifiedByUser = new HashSet<Accounts>();
        //        BranchCreatedByUser = new HashSet<Branch>();
        //        BranchModifiedByUser = new HashSet<Branch>();
        //        CategoryCreatedByUser = new HashSet<Category>();
        //        CategoryModifiedByUser = new HashSet<Category>();

        //        CircleCreatedByUser = new HashSet<Circle>();
        //        CircleModifiedByUser = new HashSet<Circle>();
        //        DesignationCreatedByUser = new HashSet<Designation>();
        //        DesignationModifiedByUser = new HashSet<Designation>();
        //        ImageCreatedByUser = new HashSet<Image>();
        //        ImageModifiedByuser = new HashSet<Image>();
        //        MemberCreatedByUser = new HashSet<Member>();
        //        MemberModifiedByUser = new HashSet<Member>();

        //        StatusCreatedByUser = new HashSet<Status>();
        //        StatusModifiedByUser = new HashSet<Status>();
        //        TransferCreatedByUser = new HashSet<Transfer>();
        //        TransferModifiedByUser = new HashSet<Transfer>();
        //    }

            public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public string EmailId { get; set; }

        public string PhoneNum { get; set; }

        public string UserStatus { get; set; }

        public int? UserTypeId { get; set; }
        public int? EmployeeId { get; set; }

        public bool IsActive { get; set; }

        public bool IsLocked { get; set; } = false;
        public int? WrongTryCount { get; set; } = 0;


        public DateTime? LastLoggedTime { get; set; }

        //public DateTime? ValidTill { get; set; }
        //public int? CreatedByUserId { get; set; }
        //public DateTime? CreatedDate { get; set; }
        //public int? ModifiedByUserId { get; set; }
        //public DateTime? ModifiedDate { get; set; }

        public virtual ICollection<Accounts> AccountsCreatedByUser { get; set; }
        public virtual ICollection<Accounts> AccountsModifiedByUser { get; set; }
        public virtual ICollection<Branch> BranchCreatedByUser { get; set; }
        public virtual ICollection<Branch> BranchModifiedByUser { get; set; }
        public virtual ICollection<Category> CategoryCreatedByUser { get; set; }
        public virtual ICollection<Category> CategoryModifiedByUser { get; set; }
        public virtual ICollection<Circle> CircleCreatedByUser { get; set; }
        public virtual ICollection<Circle> CircleModifiedByUser { get; set; }
        public virtual ICollection<Designation> DesignationCreatedByUser { get; set; }
        public virtual ICollection<Designation> DesignationModifiedByUser { get; set; }
        //public virtual ICollection<Image> ImageCreatedByUser { get; set; }
        //public virtual ICollection<Image> ImageModifiedByuser { get; set; }
        public virtual ICollection<Member> MemberCreatedByUser { get; set; }
        public virtual ICollection<Member> MemberModifiedByUser { get; set; }

        public virtual ICollection<Status> StatusCreatedByUser { get; set; }
        public virtual ICollection<Status> StatusModifiedByUser { get; set; }
        //public virtual ICollection<Transfer> TransferCreatedByUser { get; set; }
        //public virtual ICollection<Transfer> TransferModifiedByUser { get; set; }

        [NotMapped]
        public string EmployeeName { get; set; }

        [NotMapped]
        public string UserTypeName { get; set; }


    }
}


