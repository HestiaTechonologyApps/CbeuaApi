using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Core.Helpers
{
    public static class CustomDateHelper
    {
        private const string DefaultTimeZoneId = "Asia/Kolkata"; // change as needed
        private const string DefaultTimeFormat = "dd MMMM yyyy hh:mm tt"; // change as needed

        public static DateTime ConvertToUtc(DateTime localDateTime)
        {
            return TimeZoneInfo.ConvertTimeToUtc(localDateTime);
        }

        public static DateTime? ConvertToLocal(DateTime? utcDateTime, string timeZoneId="")
        {
            if (utcDateTime == null) return null;

            if (string.IsNullOrEmpty(timeZoneId))
                timeZoneId = DefaultTimeZoneId;

            TimeZoneInfo timeZone;
            try
            {
                timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            }
            catch (TimeZoneNotFoundException)
            {
                timeZone = TimeZoneInfo.FindSystemTimeZoneById(DefaultTimeZoneId);
            }
            catch (InvalidTimeZoneException)
            {
                timeZone = TimeZoneInfo.FindSystemTimeZoneById(DefaultTimeZoneId);
            }

            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime.Value, timeZone);
        }

        public static string ConvertToLocalTimeFormat(DateTime? utcDateTime, string timeZoneId="")
        {
            var localTime = ConvertToLocal(utcDateTime, timeZoneId);
            return localTime.HasValue ? localTime.Value.ToString(DefaultTimeFormat) : "";

        }
    }
}
