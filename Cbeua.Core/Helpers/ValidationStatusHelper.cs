using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cbeua.Domain.Constants;

namespace Cbeua.InfraCore.External
{
    public static class ValidationStatusHelper
    {
        public static (int Code, string Message) GetStatusInfo(ValidationStatusCode status)
        {
            return status switch
            {
                ValidationStatusCode.Success => (1000, "Validation successful."),
                ValidationStatusCode.InvalidOtp => (1001, "Invalid OTP entered."),
                ValidationStatusCode.OtpExpired => (1002, "OTP has expired."),
                ValidationStatusCode.AccountLocked => (1003, "Account is locked due to multiple failed attempts."),
                ValidationStatusCode.TooManyAttempts => (1004, "Too many OTP attempts. Please try later."),
                ValidationStatusCode.MobileNotRegistered => (1005, "Mobile number not registered."),
                ValidationStatusCode.MaxOTPresendsreachedfortoday => (1006, "Max OTP resends reached for today.."),
                ValidationStatusCode.UserLockedOrNotFound => (1006, "User Locked Or Not Found.."),
                ValidationStatusCode.OTPExpired => (1006, "OTP Expired.."),
                ValidationStatusCode.WrongOTP => (1006, "Wrong OTP.."),



                _ => (-1, "Unknown validation status.")
            };
        }
    }
}
