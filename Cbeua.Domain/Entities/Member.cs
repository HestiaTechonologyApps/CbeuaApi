using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Cbeua.Domain.Entities
{
    public class Member
    {


        public Member()
        {
        }

        public int MemberId { get; set; }
        public int StaffNo { get; set; }
        public string Name { get; set; }
        public int GenderId { get; set; }
        public int? ImageId { get; set; }
        public DateTime? Dob { get; set; }
        public int? DesignationId { get; set; }
        public int? CategoryId { get; set; }
        public DateTime? Doj { get; set; }
        public int? DpCode { get; set; }
        public DateTime? DojtoScheme { get; set; }
        public int StatusId { get; set; }
        public bool IsRegCompleted { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int ModifiedByUserId { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public string Nominee { get; set; }

        public string NomineeRelation { get; set; }

        public string NomineeIDentity { get; set; }


        public string UnionMember { get; set; }

        [NotMapped]
        public string Gender_text { get; set; }
        [NotMapped]
        public string Designation_text { get; set; }
        [NotMapped]
        public string Category_text { get; set; }
        [NotMapped]
        public string Status_text { get; set; }
        [NotMapped]
        public string TotalContribution { get; set; }

        [NotMapped]
        public string TotalRefund { get; set; }

        public partial class Branch
        {
            public Branch()
            {
                // Accounts = new HashSet<Accounts>();
                //  Member = new HashSet<Member>();
                //  Transfer = new HashSet<Transfer>();
            }

            public int BranchId { get; set; }
            public int DpCode { get; set; }
            public int CircleId { get; set; }
            public int? StateId { get; set; }
            public string Name { get; set; }
            public string Address1 { get; set; }
            public string Address2 { get; set; }
            public string Address3 { get; set; }
            public string District { get; set; }

            public bool IsRegCompleted { get; set; }

            public string Status { get; set; }

            public virtual State State { get; set; }

            [NotMapped]
            public string Circle_text { get; set; }
            [NotMapped]
            public string State_text { get; set; }


        }
        public partial class UserType
        {
            public int Id { get; set; }
            public string Abbreviation { get; set; }
            public string Description { get; set; }
        }
        public class YearMaster
        {
            [Key]
            public int YearOf { get; set; }

            public int YearName { get; set; }

        }
        public partial class Status
        {
            public Status()
            {
                // Accounts = new HashSet<Accounts>();
                // Member = new HashSet<Member>();
            }

            public int Id { get; set; }
            public string Name { get; set; }
            public string Abbreviation { get; set; }
            public string Description { get; set; }
            public int? GroupId { get; set; }



        }
        public partial class State
        {
            public State()
            {
                //  Branch = new HashSet<Branch>();
                //  Circle = new HashSet<Circle>();
            }

            public int StateId { get; set; }
            public string Name { get; set; }
            public string Abbreviation { get; set; }
            public bool IsActive { get; set; }

        }
        public partial class Month
        {
            public Month()
            {

            }
            public int MonthCode { get; set; }
            public string MonthName { get; set; }
            public string Abbrivation { get; set; }

        }
        public partial class Designation
        {
            public Designation()
            {
                Member = new HashSet<Member>();
            }

            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public int? CreatedByUserId { get; set; }
            public DateTime? CreatedDate { get; set; }
            public int? ModifiedByUserId { get; set; }
            public DateTime? ModifiedDate { get; set; }

            public virtual User CreatedByUser { get; set; }
            public virtual User ModifiedByUser { get; set; }
            public virtual ICollection<Member> Member { get; set; }
        }

        public partial class Circle
        {
            public Circle()
            {
                Accounts = new HashSet<Accounts>();
                Branch = new HashSet<Branch>();

            }

            public int Id { get; set; }
            public int? CircleCode { get; set; }
            public string Name { get; set; }
            public string Abbreviation { get; set; }
            public bool IsActive { get; set; }
            public int? StateId { get; set; }
            public DateTime? DateFrom { get; set; }
            public DateTime? DateTo { get; set; }
            public int CreatedByUserId { get; set; }
            public DateTime CreatedDate { get; set; }
            public int ModifiedByUserId { get; set; }
            public DateTime ModifiedDate { get; set; }

            public virtual User CreatedByUser { get; set; }
            public virtual User ModifiedByUser { get; set; }
            public virtual State State { get; set; }
            public virtual ICollection<Accounts> Accounts { get; set; }
            public virtual ICollection<Branch> Branch { get; set; }

        }
        public partial class Category
        {
            public Category()
            {
                Member = new HashSet<Member>();
            }

            public int Id { get; set; }
            public string Name { get; set; }
            public string Abbreviation { get; set; }

            public virtual ICollection<Member> Member { get; set; }
        }
        //public partial class Branch
        //{
        //    public Branch()
        //    {
        //        Accounts = new HashSet<Accounts>();
        //        Member = new HashSet<Member>();
        //        Transfer = new HashSet<Transfer>();
        //    }

        //    public int Id { get; set; }
        //    public int DpCode { get; set; }
        //    public int CircleId { get; set; }
        //    public int? StateId { get; set; }
        //    public string Name { get; set; }
        //    public string Address1 { get; set; }
        //    public string Address2 { get; set; }
        //    public string Address3 { get; set; }
        //    public string District { get; set; }

        //    public bool IsRegCompleted { get; set; }

        //    public string Status { get; set; }
        //    public virtual Circle Circle { get; set; }

        //    public virtual State State { get; set; }
        //    public virtual ICollection<Accounts> Accounts { get; set; }
        //    public virtual ICollection<Member> Member { get; set; }
        //    public virtual ICollection<Transfer> Transfer { get; set; }

        //    [NotMapped]
        //    public string Circle_text { get; set; }
        //    [NotMapped]
        //    public string State_text { get; set; }


        //}

        public class RefundContribution
        {


            public int Id { get; set; }
            public int StaffNo { get; set; }

            public int? StateId { get; set; }

            public int? DesignationId { get; set; }
            public DateTime? DeathDate { get; set; }

            public String RefundNO { get; set; }
            public String BranchNameOFTime { get; set; }
            public String DPCODEOfTime { get; set; }

            public String Type { get; set; }

            public String Remark { get; set; }
            public string DDNO { get; set; }

            public DateTime? DDDATE { get; set; }


            public decimal Amount { get; set; }

            public float LastContribution { get; set; }


            public int YearOF { get; set; }

            public virtual State State { get; set; }
            public virtual Member StaffNoNavigation { get; set; }

            public virtual Designation Designation { get; set; }
        }

        public class DeathClaim
        {


            public int Id { get; set; }
            public int StaffNo { get; set; }

            public int? StateId { get; set; }

            public int? DesignationId { get; set; }
            public DateTime? DeathDate { get; set; }

            public string Nominee { get; set; }

            public string NomineeRelation { get; set; }

            public string NomineeIDentity { get; set; }


            public string DDNO { get; set; }

            public DateTime? DDDATE { get; set; }


            public decimal Amount { get; set; }

            public float LastContribution { get; set; }


            public int YearOF { get; set; }

            public virtual State State { get; set; }
            public virtual Member StaffNoNavigation { get; set; }

            public virtual Designation Designation { get; set; }
        }
        public partial class Accounts
        {
            public long Id { get; set; }
            public int CircleId { get; set; }
            public int? DpCode { get; set; }
            public int StaffNo { get; set; }
            public int MonthCode { get; set; }
            public int YearOf { get; set; }
            public decimal Amount { get; set; }
            public int TransMode { get; set; }
            public int CreatedByUserId { get; set; }
            public DateTime CreatedDate { get; set; }
            public int ModifiedByUserId { get; set; }
            public DateTime ModifiedDate { get; set; }




            public string Reference { get; set; }
            public string Remark { get; set; }
            public virtual Circle Circle { get; set; }
            public virtual User CreatedByUser { get; set; }
            public virtual Branch DpCodeNavigation { get; set; }
            public virtual User ModifiedByUser { get; set; }
            public virtual Member StaffNoNavigation { get; set; }
            public virtual Status TransModeNavigation { get; set; }


        }
        public partial class AccountsDirectEntry
        {
            public int ID { get; set; }
            public double? StaffNo { get; set; }
            public string Name { get; set; }
            public double? DpCode { get; set; }

            public int MonthCode { get; set; }
            public int YearOf { get; set; }
            public string DdIba { get; set; }
            public string DdIbaDate { get; set; }
            public double? Amt { get; set; }
            public string Enrl { get; set; }
            public string Fine { get; set; }
            public string F9 { get; set; }
            public string F10 { get; set; }
            public string F11 { get; set; }

            public string status { get; set; }
            public Boolean isApproved { get; set; }
            public String ApprovedBy { get; set; }
            public String ApprovedDate { get; set; }
        }

        public class ContributionDetail
        {
            public ContributionDetail()
            {
                isParked = false;
            }
            public long Id { get; set; }
            public String FullString { get; set; }
            public int Circle { get; set; }
            public String Month { get; set; }
            public String Year { get; set; }
            public String DpCode { get; set; }
            public String StaffNo { get; set; }
            public String Name { get; set; }
            public String Designation { get; set; }

            public Decimal Amount { get; set; }
            public Boolean isParked { get; set; }

            public long ContributionMasterId { get; set; }

            public String ParkReason { get; set; } = "";

            public DateTime? Parkedon { get; set; }

            public DateTime? UnParkedon { get; set; }

            public String Total { get; set; }
        }

        public class ContributionMaster
        {
            public ContributionMaster()
            {
                // ContributionDetails = new HashSet<ContributionDetail>();

            }
            public long Id { get; set; }
            public String FileName { get; set; }
            public String FileLocation { get; set; }

            public String FileType { get; set; }

            public String FileExtension { get; set; }

            public Decimal FileSize { get; set; }



            public String Month { get; set; }
            public String Year { get; set; }

            public string Circle { get; set; }

            public String totalamount { get; set; }
            public String totalentry { get; set; }
            public String NewMemberCount { get; set; }


            public string ContributionStatus { get; set; }



            public Boolean isApproved { get; set; }

            public String ApprovedBy { get; set; }
            public String ApprovedDate { get; set; }

            public virtual ICollection<ContributionDetail> ContributionDetails { get; set; }
        }

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
    

