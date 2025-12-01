using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Cbeua.Business.Services;
using Cbeua.Core.Helpers;
using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IServices;
using CbeuaAPI.Controllers;

namespace Cbeua.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuditLogController : Api_BaseController
    {
        private readonly IAuditLogService _auditlogService;
        private readonly IWebHostEnvironment _env;
        public AuditLogController(IAuditLogService attachmentService, IWebHostEnvironment env)
        {
            _auditlogService = attachmentService;
            _env = env;
        }
        [HttpGet("{tableName}/{recordId}")]
        public async Task<CustomApiResponse> GetAuditLog(string tableName, int recordId)
        {
            var response = await _auditlogService.GetAuditLogsForEntityAsync(tableName, recordId);
            return ApiResponseFactory.Success(response);
        }
    }
}