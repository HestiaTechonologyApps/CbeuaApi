namespace Cbeua.Domain.Entities
{
    public class ContributionMaster
    {
        public ContributionMaster()
        {
            // ContributionDetails = new HashSet<ContributionDetail>();

        }
        public long ContributionMasterId { get; set; }
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
}


