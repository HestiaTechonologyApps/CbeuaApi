using Cbeua.Domain.DTO;
using Cbeua.Domain.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Cbeua.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactMessageController : ControllerBase
    {
        private readonly IContactMessageService _service;

        public ContactMessageController(IContactMessageService service)
        {
            _service = service;
        }

        /// <summary>
        /// Public endpoint to submit contact form (No authentication required)
        /// </summary>
        [HttpPost("submit")]
        [AllowAnonymous]
        public async Task<CustomApiResponse> SubmitContactForm([FromBody] ContactFormSubmissionDTO submission)
        {
            var response = new CustomApiResponse();
            try
            {
                // Get client IP address
                string? ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

                var created = await _service.SubmitContactFormAsync(submission, ipAddress);
                response.IsSucess = true;
                response.Value = new
                {
                    message = "Your message has been sent successfully! We will get back to you soon.",
                    contactMessageId = created.ContactMessageId,
                    submittedAt = created.SubmittedAt
                };
                response.StatusCode = 201;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Controller Error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");

                response.IsSucess = false;
                response.Error = "Failed to send message. Please try again later.";
                response.StatusCode = 500;
            }
            return response;
        }

        /// <summary>
        /// Get all contact messages (Admin only)
        /// </summary>
        [HttpGet]
        [Authorize]
        public async Task<CustomApiResponse> GetAll()
        {
            var response = new CustomApiResponse();
            try
            {
                var contactMessages = await _service.GetAllAsync();
                response.IsSucess = true;
                response.Value = contactMessages;
                response.StatusCode = 200;
            }
            catch (Exception ex)
            {
                response.IsSucess = false;
                response.Error = ex.Message;
                response.StatusCode = 500;
            }
            return response;
        }

        /// <summary>
        /// Get contact message by ID (Admin only)
        /// </summary>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<CustomApiResponse> GetById(int id)
        {
            var response = new CustomApiResponse();
            try
            {
                var contactMessage = await _service.GetByIdAsync(id);
                if (contactMessage == null)
                {
                    response.IsSucess = false;
                    response.Error = "Contact message not found";
                    response.StatusCode = 404;
                }
                else
                {
                    response.IsSucess = true;
                    response.Value = contactMessage;
                    response.StatusCode = 200;
                }
            }
            catch (Exception ex)
            {
                response.IsSucess = false;
                response.Error = ex.Message;
                response.StatusCode = 500;
            }
            return response;
        }

        /// <summary>
        /// Mark message as read (Admin only)
        /// </summary>
        [HttpPatch("{id}/mark-read")]
        [Authorize]
        public async Task<CustomApiResponse> MarkAsRead(int id)
        {
            var response = new CustomApiResponse();
            try
            {
                var updated = await _service.MarkAsReadAsync(id);
                if (!updated)
                {
                    response.IsSucess = false;
                    response.Error = "Contact message not found";
                    response.StatusCode = 404;
                }
                else
                {
                    response.IsSucess = true;
                    response.Value = new { message = "Message marked as read successfully" };
                    response.StatusCode = 200;
                }
            }
            catch (Exception ex)
            {
                response.IsSucess = false;
                response.Error = ex.Message;
                response.StatusCode = 500;
            }
            return response;
        }

        /// <summary>
        /// Mark message as replied with optional admin notes (Admin only)
        /// </summary>
        [HttpPatch("{id}/mark-replied")]
        [Authorize]
        public async Task<CustomApiResponse> MarkAsReplied(int id, [FromBody] MarkAsRepliedRequest? request = null)
        {
            var response = new CustomApiResponse();
            try
            {
                var adminNotes = request?.AdminNotes;
                var updated = await _service.MarkAsRepliedAsync(id, adminNotes);

                if (!updated)
                {
                    response.IsSucess = false;
                    response.Error = "Contact message not found";
                    response.StatusCode = 404;
                }
                else
                {
                    response.IsSucess = true;
                    response.Value = new { message = "Message marked as replied successfully" };
                    response.StatusCode = 200;
                }
            }
            catch (Exception ex)
            {
                response.IsSucess = false;
                response.Error = ex.Message;
                response.StatusCode = 500;
            }
            return response;
        }
    }

    /// <summary>
    /// Request model for mark-replied endpoint
    /// </summary>
    public class MarkAsRepliedRequest
    {
        public string? AdminNotes { get; set; }
    }
}