using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cbeua.Domain.DTO; // For CustomApiResponse
using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IServices;
using CbeuaAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class AuditController : Api_BaseController
{
    private readonly IAuditLogService _service;
    public AuditController(IAuditLogService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<CustomApiResponse> GetAll()
    {
        var response = new CustomApiResponse();
        try
        {
            var categoryTaxs = await _service.GetAllLogsAsync();
            response.IsSucess = true;
            response.Value = categoryTaxs;
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

 
}
