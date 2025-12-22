using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.Entities
{
    public class DirectPayment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DirectPaymentId { get; set; }

        public int MemberId { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; }

        public string PaymentMode { get; set; } = "";// Cash / Bank / UPI

        public string ReferenceNo { get; set; } = "";

        public string Remarks { get; set; } = "";

        public int CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }

        public bool IsDeleted { get; set; }
    }
}
