namespace Cbeua.Domain.Configurations
{

    public class OtpSettings
    {
        public int OTPExpiryMinutes { get; set; }
        public int MaxWrongAttempts { get; set; }
        public int MaxResendCountPerDay { get; set; }
        public int ResendCooldownSeconds { get; set; }
        public int OTPLength { get; set; }
    }
}