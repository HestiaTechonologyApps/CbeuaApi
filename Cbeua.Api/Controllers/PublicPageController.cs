using Cbeua.Domain.DTO;
using Cbeua.Domain.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cbeua.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class PublicPageController : ControllerBase
    {
        private readonly IPublicPageService _service;
        public PublicPageController(IPublicPageService service)
        {
            _service = service;
        }

        [HttpGet("public/home")]
        public async Task<CustomApiResponse> GetAll()
        {
            var response = new CustomApiResponse();
            try
            {
                var publicPages = await _service.GetAllAsync();
                response.IsSucess = true;
                response.Value = publicPages;
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
}
