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
public class CompanyController : Api_BaseController
{
    private readonly ICompanyService _service;
    public CompanyController(ICompanyService service)
    {
        _service = service;
    }

    [HttpGet("admin-getall-company")]
    public async Task<CustomApiResponse> GetAll()
    {
        var response = new CustomApiResponse();
        try
        {
            var companys = await _service.GetAllAsync();
            response.IsSucess = true;
            response.Value = companys;
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
    /// Give the list of company and their ID for look up purpose
    /// </summary>
    /// <returns></returns>
    [HttpGet("admin-lookUp-company")]
    public async Task<CustomApiResponse> GetCompanyLookUp()
    {
        var response = new CustomApiResponse();
        try
        {
            var companys = await _service.GetAllAsync();

            var lookup = new List<LookUpDTO>();
            foreach (var company in companys)
            {
                lookup.Add(new  LookUpDTO{ Id= company.CompanyId,Text= company.ComapanyName  });
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
        var company = await _service.GetByIdAsync(id);
        if (company == null)
        {
            response.IsSucess = false;
            response.Error = "Not found";
            response.StatusCode = 404;
        }
        else
        {
            response.IsSucess = true;
            response.Value = company;
            response.StatusCode = 200;
        }
        return response;
    }

    [HttpPost]
    public async Task<CustomApiResponse> Create([FromBody] Company company)
    {
        var response = new CustomApiResponse();
        try
        {
            var created = await _service.CreateAsync(company);
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
    public async Task<CustomApiResponse> Update(int id, [FromBody] Company company)
    {
        var response = new CustomApiResponse();
        if (id != company.CompanyId)
        {
            response.IsSucess = false;
            response.Error = "Id mismatch";
            response.StatusCode = 400;
            return response;
        }
        var updated = await _service.UpdateAsync(company);
        if (!updated)
        {
            response.IsSucess = false;
            response.Error = "Not found";
            response.StatusCode = 404;
        }
        else
        {
            response.IsSucess = true;
            response.Value = company;
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
