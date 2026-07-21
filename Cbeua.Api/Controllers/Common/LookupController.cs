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
    [Authorize]
    public class LookUpController : ControllerBase
    {
        private readonly ILookupService _lookupService;

        public LookUpController(ILookupService lookupService)
        {
            _lookupService = lookupService;
        }

        [HttpGet("paged")]
        public async Task<CustomApiResponse> GetPagedLookup([FromQuery] LookupPaginationParams parameters)
        {
            try
            {
                return await _lookupService.GetPagedLookupAsync(parameters);
            }
            catch (Exception ex)
            {
                return new CustomApiResponse { IsSucess = false, Error = ex.Message, StatusCode = 500 };
            }
        }
    }
}