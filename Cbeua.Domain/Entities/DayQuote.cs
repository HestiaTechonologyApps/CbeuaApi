using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.Entities
{
    public class DayQuote
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DayQuoteId { get; set; }
        public int Day { get; set; }
        public int MonthCode { get; set; }
        public String ToDayQuote { get; set; } = "";
        public String UnformatedContent { get; set; } = "";
    }
}
