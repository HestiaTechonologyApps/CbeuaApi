using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Cbeua.Domain.DTO;
using Cbeua.Domain.Interfaces.IServices;
using Cbeua.Domain.Entities.Common;

namespace Cbeua.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }
        // Get attachments by RecordID
        [HttpGet("{tableName}/{recordId}")]
        public async Task<CustomApiResponse> GetAttachments(string tableName, int recordId)
        {
            var response = await _commentService.GetCommentAsync(tableName, recordId);
            return response;
        }

        [HttpGet("{id}")]
        public async Task<CustomApiResponse> GetById(int id)
        {
            var response = new CustomApiResponse();
            var driver = await _commentService.GetByIdAsync(id);
            if (driver == null)
            {
                response.IsSucess = false;
                response.Error = "Not found";
                response.StatusCode = 404;
            }
            else
            {
                response.IsSucess = true;
                response.Value = driver;
                response.StatusCode = 200;
            }
            return response;
        }




        // Soft delete attachment
        [HttpDelete("{CommentId}")]
        public async Task<CustomApiResponse> Delete(int commentId, [FromQuery] string deletedBy)
        {
            var response = await _commentService.DeleteCommentAsync(commentId, deletedBy);
            return response;

        }
        [HttpPost("AddComment")]
        public async Task<CustomApiResponse> AddComment([FromBody] Comment comment)
        {
            var response = new CustomApiResponse();
            try
            {
                var created = await _commentService.CreateAsync(comment);
                response.IsSucess = true;
                response.Value = created;
                response.StatusCode = 201;
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
}
