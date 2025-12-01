namespace Cbeua.InfraCore.External
{


    public interface ITwilioSmsSender
    {
        Task<bool> SendSmsAsync(string to, string message);
    }

    public class TwilioSmsSender : ITwilioSmsSender
    {
        public async Task<bool> SendSmsAsync(string to, string message)
        {
            // Correcting the return statement to properly use async/await
            await Task.Yield(); // Simulating asynchronous operation
            return true;
        }
    }
}