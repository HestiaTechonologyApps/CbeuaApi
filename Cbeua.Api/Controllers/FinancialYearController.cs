using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cbeua.Domain.DTO; // For CustomApiResponse
using Cbeua.Domain.Interfaces.IServices;
using CbeuaAPI.Controllers;
using Cbeua.Domain.Entities.Common;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class FinancialYearController : Api_BaseController
{
    private readonly IFinancialYearService _service;
    public FinancialYearController(IFinancialYearService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<CustomApiResponse> GetAll()
    {
        var response = new CustomApiResponse();
        try
        {
            var financialyears = await _service.GetAllAsync();
            response.IsSucess = true;
            response.Value = financialyears;
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


    [HttpGet("admin-lookUp-financialYear")]
    public async Task<CustomApiResponse> GetCategoryLookUp()
    {
        var response = new CustomApiResponse();
        try
        {
            var categories = await _service.GetAllAsync();

            var lookup = new List<LookUpDTO>();
            foreach (var cat in categories)
            {
                lookup.Add(new LookUpDTO { Id = cat.FinancialYearId, Text = cat.FinacialYearCode, IsSelected  = cat.IsCurrent });
            }
            response.IsSucess = true;
            response.Value = lookup;
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


    [HttpGet("{id}")]
    public async Task<CustomApiResponse> GetById(int id)
    {
        var response = new CustomApiResponse();
        var financialyear = await _service.GetByIdAsync(id);
        if (financialyear == null)
        {
            response.IsSucess = false;
            response.Error = "Not found";
            response.StatusCode = 404;
        }
        else
        {
            response.IsSucess = true;
            response.Value = financialyear;
            response.StatusCode = 200;
        }
        return response;
    }

    [HttpPost]
    public async Task<CustomApiResponse> Create([FromBody] FinancialYear financialyear)
    {
        var response = new CustomApiResponse();
        try
        {
            var created = await _service.CreateAsync(financialyear);
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

    [HttpPut("{id}")]
    public async Task<CustomApiResponse> Update(int id, [FromBody] FinancialYear financialyear)
    {
        var response = new CustomApiResponse();
        if (id != financialyear.FinancialYearId)
        {
            response.IsSucess = false;
            response.Error = "Id mismatch";
            response.StatusCode = 400;
            return response;
        }
        var updated = await _service.UpdateAsync(financialyear);
        if (!updated)
        {
            response.IsSucess = false;
            response.Error = "Not found";
            response.StatusCode = 404;
        }
        else
        {
            response.IsSucess = true;
            response.Value = financialyear;
            response.StatusCode = 200;
        }
        return response;
    }

    [HttpDelete("{id}")]
    public async Task<CustomApiResponse> Delete(int id)
    {
        var response = new CustomApiResponse();
        var deleted = await _service.DeleteAsync(id);
        if (!deleted)
        {
            response.IsSucess = false;
            response.Error = "Not found";
            response.StatusCode = 404;
        }
        else
        {
            response.IsSucess = true;
            response.Value = null;
            response.StatusCode = 204;
        }
        return response;
    }
}
