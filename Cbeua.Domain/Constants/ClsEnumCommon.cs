using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.Constants
{
    class ClsEnumCommon
    {
    }
    public enum ValidationStatusCode
    {
        Success = 1000,
        InvalidOtp = 1001,
        OtpExpired = 1002,
        AccountLocked = 1003,
        TooManyAttempts = 1004,
        MobileNotRegistered = 1005,
        MaxOTPresendsreachedfortoday = 1006,
        UserLockedOrNotFound=1007,
        OTPExpired=1008,
        WrongOTP=1009

    }
}
