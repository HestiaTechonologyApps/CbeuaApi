using Cbeua.Domain.DTO;
using Cbeua.Domain.Interfaces.IServices;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Cbeua.Business.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendContactFormEmailAsync(ContactFormSubmissionDTO contactForm, string recipientEmail)
        {
            try
            {
                var smtpHost = _configuration["EmailSettings:SmtpHost"];
                var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
                var smtpUsername = _configuration["EmailSettings:SmtpUsername"];
                var smtpPassword = _configuration["EmailSettings:SmtpPassword"];
                var fromEmail = _configuration["EmailSettings:FromEmail"];
                var fromName = _configuration["EmailSettings:FromName"];

                using (var smtpClient = new SmtpClient(smtpHost, smtpPort))
                {
                    smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                    smtpClient.EnableSsl = true;

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(fromEmail, fromName),
                        Subject = $"Contact Form: {contactForm.Subject}",
                        Body = GenerateEmailBody(contactForm),
                        IsBodyHtml = true
                    };

                    mailMessage.To.Add(recipientEmail);
                    mailMessage.ReplyToList.Add(new MailAddress(contactForm.EmailAddress, contactForm.FullName));

                    await smtpClient.SendMailAsync(mailMessage);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email sending failed: {ex.Message}");
                return false;
            }
        }

        private string GenerateEmailBody(ContactFormSubmissionDTO contactForm)
        {
            return $@"
                <html>
                <body style='font-family: Arial, sans-serif;'>
                    <h2 style='color: #1e3a8a;'>New Contact Form Submission</h2>
                    <table style='border-collapse: collapse; width: 100%;'>
                        <tr>
                            <td style='padding: 10px; border: 1px solid #ddd; background-color: #f0f0f0; font-weight: bold;'>Full Name:</td>
                            <td style='padding: 10px; border: 1px solid #ddd;'>{contactForm.FullName}</td>
                        </tr>
                        <tr>
                            <td style='padding: 10px; border: 1px solid #ddd; background-color: #f0f0f0; font-weight: bold;'>Phone Number:</td>
                            <td style='padding: 10px; border: 1px solid #ddd;'>{contactForm.PhoneNumber}</td>
                        </tr>
                        <tr>
                            <td style='padding: 10px; border: 1px solid #ddd; background-color: #f0f0f0; font-weight: bold;'>Email Address:</td>
                            <td style='padding: 10px; border: 1px solid #ddd;'>{contactForm.EmailAddress}</td>
                        </tr>
                        <tr>
                            <td style='padding: 10px; border: 1px solid #ddd; background-color: #f0f0f0; font-weight: bold;'>Subject:</td>
                            <td style='padding: 10px; border: 1px solid #ddd;'>{contactForm.Subject}</td>
                        </tr>
                        <tr>
                            <td style='padding: 10px; border: 1px solid #ddd; background-color: #f0f0f0; font-weight: bold;'>Message:</td>
                            <td style='padding: 10px; border: 1px solid #ddd;'>{contactForm.Message.Replace("\n", "<br>")}</td>
                        </tr>
                    </table>
                </body>
                </html>
            ";
        }
    }
}