using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.DTO
{
    public class DirectPaymentDTO
    {
        public int DirectPaymentId { get; set; }

        public int MemberId { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; }
        public string PaymentDatestring => PaymentDate.ToString("dd MMMM yyyy hh:mm tt") ?? "";

        public string PaymentMode { get; set; } = "";// Cash / Bank / UPI

        public string ReferenceNo { get; set; } = "";

        public string Remarks { get; set; } = "";

        public int CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedDatestring => CreatedDate.ToString("dd MMMM yyyy hh:mm tt") ?? "";

        public bool IsDeleted { get; set; }
    }
}
