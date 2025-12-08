using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.DTO
{
    public class MainPageDTO
    {
        public int MainPageId { get; set; }


        public String CorouselImage1 { get; set; } = "";
        public String CorouselImage2 { get; set; } = "";
        public String CorouselImage3 { get; set; } = "";
        public String MainText { get; set; } = "";
        public String Slogan { get; set; } = "";

        public String LogoImage1 { get; set; } = "";

        public String LogoImage2 { get; set; } = "";

        public String ContactDesc1 { get; set; } = "";

        public String ContactDesc2 { get; set; } = "";

        public String ContactLine1 { get; set; } = "";

        public String ContactLine2 { get; set; } = "";

        public String ContactLine3 { get; set; } = "";

        public String Phonenum { get; set; } = "";

        public String Faxnum { get; set; } = "";


        public String Website { get; set; } = "";
        public String Email { get; set; } = "";

        public String RulesRegulation { get; set; } = "";
        public String DayQuote { get; set; } = "";
        public int CompanyId { get; set; } = 0;
        public string CompanyName { get; set; } = "";
    }
}
