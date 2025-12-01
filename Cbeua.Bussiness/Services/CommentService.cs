using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cbeua.Core.Helpers;
using Cbeua.Domain.DTO;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.Domain.Interfaces.IServices;
using Cbeua.Domain.Entities.Common;

namespace Cbeua.Bussiness.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _repo;
        public CommentService(ICommentRepository repo)
        {
            _repo = repo;
        }

        public async Task<CustomApiResponse> AddCommentAsync(string description, string tableName, int recordId, string createdBy, bool isInternal, int parentCommentId = 0)
        {
            {
                try
                {
                 
                    var newComment = new Comment
                    {
                        Description = description,
                        TableName = tableName,
                        RecordID = recordId,
                        CreatedBy = createdBy,
                        IsInternal = isInternal,
                        ParentCommentId = parentCommentId,
                        CreatedOn = DateTime.Now,
                        IsDeleted = false
                    };

                    await _repo.AddAsync(newComment);
                    await _repo.SaveChangesAsync();

                    return new CustomApiResponse
                    {
                        StatusCode = 200,
                        IsSucess = true,
                        CustomMessage = "Comment added successfully",
                        Value = newComment
                    };
                }
                catch (Exception ex)
                {
                    return new CustomApiResponse
                    {
                        StatusCode = 500,
                        IsSucess = false,
                        CustomMessage = "Failed to add comment",
                        Error = ex.InnerException?.Message ?? ex.Message
                    };
                }
            }
        }

        public async Task<Comment> CreateAsync(Comment comment)
        {
            await _repo.AddAsync(comment);
            await _repo.SaveChangesAsync();
            return comment;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var comment = await _repo.GetByIdAsync(id);
            if (comment == null) return false;
            _repo.Delete(comment);
            await _repo.SaveChangesAsync();
            return true;
        }

      

        public async Task<List<Comment>> GetAllAsync() => (List<Comment>) await _repo.GetAllAsync();


        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<CustomApiResponse> GetCommentAsync(string tableName, int recordId)
        {
            try
            {
                var allComments = await _repo.GetAllAsync();

                var comments = allComments
                    .Where(c => c.TableName == tableName
                             && c.RecordID == recordId      // ✔ filter by record ID
                             && c.IsDeleted == false)
                    .OrderByDescending(c => c.CreatedOn)
                    .ToList();

                return new CustomApiResponse
                {
                    StatusCode = 200,
                    IsSucess = true,
                    CustomMessage = "Comments fetched successfully",
                    Value = comments
                };
            }
            catch (Exception ex)
            {
                return new CustomApiResponse
                {
                    StatusCode = 500,
                    IsSucess = false,
                    CustomMessage = "Failed to load comments",
                    Error = ex.InnerException?.Message ?? ex.Message
                };
            }
        }


        public async Task<bool> UpdateAsync(Comment comment)
        {
            _repo.Update(comment);
            await _repo.SaveChangesAsync();
            return true;
        }


        public async Task<CustomApiResponse> DeleteCommentAsync(int commentId, string deletedBy)
        {
            var comment = await _repo.GetByIdAsync(commentId);
            if (comment == null)
                return ApiResponseFactory.Fail("Comment not found", System.Net.HttpStatusCode.NotFound);


            comment.IsDeleted = true;
            comment.DeletedOn = DateTime.UtcNow;
            comment.DeletedBy = deletedBy;

            _repo.Update(comment);
            await _repo.SaveChangesAsync();
            return ApiResponseFactory.Success(null, "Comment deleted successfully", System.Net.HttpStatusCode.OK);

        }
      

    }
}
