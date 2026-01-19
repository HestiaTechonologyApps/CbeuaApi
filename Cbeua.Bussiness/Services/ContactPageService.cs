using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.Domain.Interfaces.IServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cbeua.Business.Services
{
    public class ContactMessageService : IContactMessageService
    {
        private readonly IContactMessageRepository _repo;
        private readonly IAuditRepository _auditRepository;
        private readonly IEmailService _emailService;
        private readonly IPublicPageService _publicPageService;
        public const string AuditTableName = "ContactMessages";

        public ContactMessageService(
            IContactMessageRepository repo,
            IAuditRepository auditRepository,
            IEmailService emailService,
            IPublicPageService publicPageService)
        {
            _repo = repo;
            _auditRepository = auditRepository;
            _emailService = emailService;
            _publicPageService = publicPageService;
        }

        public async Task<ContactMessageDTO> CreateAsync(ContactMessage contactMessage)
        {
            await _repo.AddAsync(contactMessage);
            await _repo.SaveChangesAsync();

            await _auditRepository.LogAuditAsync<ContactMessage>(
                tableName: AuditTableName,
                action: "create",
                recordId: contactMessage.ContactMessageId,
                oldEntity: null,
                newEntity: contactMessage,
                changedBy: "System"
            );

            return ConvertToDTO(contactMessage);
        }

        public async Task<ContactMessageDTO> SubmitContactFormAsync(ContactFormSubmissionDTO submission, string? ipAddress = null)
        {
            try
            {
                // Create and save the contact message first
                var contactMessage = new ContactMessage
                {
                    FullName = submission.FullName,
                    PhoneNumber = submission.PhoneNumber,
                    EmailAddress = submission.EmailAddress,
                    Subject = submission.Subject,
                    Message = submission.Message,
                    SubmittedAt = DateTime.Now,
                    IsRead = false,
                    IsReplied = false,
                    IPAddress = ipAddress
                };

                await _repo.AddAsync(contactMessage);
                await _repo.SaveChangesAsync();

                Console.WriteLine($"✓ Contact message saved successfully with ID: {contactMessage.ContactMessageId}");

                // Send email notification to company (non-blocking)
                try
                {
                    Console.WriteLine("→ Attempting to get public pages...");
                    var publicPages = await _publicPageService.GetAllAsync();
                    Console.WriteLine($"→ Found {publicPages?.Count ?? 0} public pages");

                    if (publicPages != null && publicPages.Count > 0)
                    {
                        var activePage = publicPages.FirstOrDefault(p => p.IsActive);
                        Console.WriteLine($"→ Active page found: {activePage != null}");

                        if (activePage != null)
                        {
                            Console.WriteLine($"→ Office Email: {activePage.OfficeEmail ?? "NULL"}");

                            if (!string.IsNullOrWhiteSpace(activePage.OfficeEmail))
                            {
                                Console.WriteLine($"→ Attempting to send email to: {activePage.OfficeEmail}");
                                var emailSent = await _emailService.SendContactFormEmailAsync(submission, activePage.OfficeEmail);

                                if (emailSent)
                                {
                                    Console.WriteLine("✓ Email sent successfully!");
                                }
                                else
                                {
                                    Console.WriteLine("✗ Email service returned false (check SMTP settings)");
                                }
                            }
                            else
                            {
                                Console.WriteLine("✗ Office email is empty or null");
                            }
                        }
                        else
                        {
                            Console.WriteLine("✗ No active public page found (IsActive = false)");
                        }
                    }
                    else
                    {
                        Console.WriteLine("✗ No public pages returned from service");
                    }
                }
                catch (Exception emailEx)
                {
                    Console.WriteLine($"✗ Email notification failed: {emailEx.Message}");
                    Console.WriteLine($"→ Error type: {emailEx.GetType().Name}");
                    if (emailEx.InnerException != null)
                    {
                        Console.WriteLine($"→ Inner exception: {emailEx.InnerException.Message}");
                    }
                }

                // Log audit (non-blocking)
                try
                {
                    await _auditRepository.LogAuditAsync<ContactMessage>(
                        tableName: AuditTableName,
                        action: "submit",
                        recordId: contactMessage.ContactMessageId,
                        oldEntity: null,
                        newEntity: contactMessage,
                        changedBy: "Public User"
                    );
                    Console.WriteLine("✓ Audit log created successfully");
                }
                catch (Exception auditEx)
                {
                    Console.WriteLine($"✗ Audit logging failed: {auditEx.Message}");
                }

                return ConvertToDTO(contactMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗✗✗ CRITICAL ERROR in SubmitContactFormAsync: {ex.Message}");
                Console.WriteLine($"→ Error type: {ex.GetType().Name}");
                Console.WriteLine($"→ Stack trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"→ Inner exception: {ex.InnerException.Message}");
                }
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var contactMessage = await _repo.GetByIdAsync(id);
            if (contactMessage == null) return false;

            _repo.Delete(contactMessage);

            await _auditRepository.LogAuditAsync<ContactMessage>(
                tableName: AuditTableName,
                action: "Delete",
                recordId: contactMessage.ContactMessageId,
                oldEntity: contactMessage,
                newEntity: null,
                changedBy: "System"
            );

            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<List<ContactMessageDTO>> GetAllAsync()
        {
            var contactMessages = _repo.GetQueryableContactMessageList();
            return await contactMessages.ToListAsync();
        }

        public async Task<ContactMessageDTO?> GetByIdAsync(int id)
        {
            var contactMessage = await _repo.GetByIdAsync(id);
            if (contactMessage == null) return null;
            return ConvertToDTO(contactMessage);
        }

        public async Task<bool> UpdateAsync(ContactMessage contactMessage)
        {
            var oldEntity = await _repo.GetByIdAsync(contactMessage.ContactMessageId);
            if (oldEntity == null) return false;

            _repo.Detach(oldEntity);
            _repo.Update(contactMessage);
            await _repo.SaveChangesAsync();

            await _auditRepository.LogAuditAsync<ContactMessage>(
                tableName: AuditTableName,
                action: "update",
                recordId: contactMessage.ContactMessageId,
                oldEntity: oldEntity,
                newEntity: contactMessage,
                changedBy: "System"
            );

            return true;
        }

        public async Task<bool> MarkAsReadAsync(int id)
        {
            var contactMessage = await _repo.GetByIdAsync(id);
            if (contactMessage == null) return false;

            contactMessage.IsRead = true;
            _repo.Update(contactMessage);
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MarkAsRepliedAsync(int id, string? adminNotes = null)
        {
            var contactMessage = await _repo.GetByIdAsync(id);
            if (contactMessage == null) return false;

            contactMessage.IsReplied = true;
            contactMessage.RepliedAt = DateTime.Now;
            contactMessage.AdminNotes = adminNotes;

            _repo.Update(contactMessage);
            await _repo.SaveChangesAsync();
            return true;
        }

        private ContactMessageDTO ConvertToDTO(ContactMessage contactMessage)
        {
            return new ContactMessageDTO
            {
                ContactMessageId = contactMessage.ContactMessageId,
                FullName = contactMessage.FullName,
                PhoneNumber = contactMessage.PhoneNumber,
                EmailAddress = contactMessage.EmailAddress,
                Subject = contactMessage.Subject,
                Message = contactMessage.Message,
                SubmittedAt = contactMessage.SubmittedAt,
                IsRead = contactMessage.IsRead,
                IsReplied = contactMessage.IsReplied,
                AdminNotes = contactMessage.AdminNotes,
                RepliedAt = contactMessage.RepliedAt,
                IPAddress = contactMessage.IPAddress
            };
        }
    }
}