using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cbeua.Domain.Entities
{

    public class Company
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompanyId { get; set; } // Primary key
        public String ComapanyName { get; set; } = "";

        public String Website { get; set; } = "";

        public String ContactNumber { get; set; } = "";

        public String Email { get; set; } = "";

        public String TaxNumber { get; set; } = "";

        public String AddressLine1 { get; set; } = "";
        public String AddressLine2 { get; set; } = "";
        public String City { get; set; } = "";
        public String State { get; set; } = "";
        public String Country { get; set; } = "";
        public String ZipCode { get; set; } = "";

        public String InvoicePrefix { get; set; } = "";
        public String CompanyLogo { get; set; } = "";

        public bool IsActive { get; set; } = true;


        public bool IsDeleted { get; set; } = false;
    }
}
