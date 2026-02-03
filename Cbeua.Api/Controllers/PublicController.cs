using Cbeua.Bussiness.Services;
using Cbeua.Core.Helpers;
using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cbeua.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class PublicController : ControllerBase
    {
        private readonly IDailyNewsService dailyNewsService ;
        private readonly IDayQuoteService  dayQuoteService;
        private readonly IMainPageService mainPageService ;
        private readonly IPublicPageService publicPageService ;
        private readonly IManagingComiteeService _manageservice;
        private readonly IAttachmentService _attachmentService;

        private readonly IDeathClaimService _deathservice;

        public PublicController(IDailyNewsService dailyNewsService ,IDayQuoteService dayQuoteService ,
            IMainPageService mainPageService , IManagingComiteeService manageservice,
            IPublicPageService publicPageService, IAttachmentService _attachmentService, IDeathClaimService deathClaimService)
        {
           this.dayQuoteService = dayQuoteService;
           this.dailyNewsService = dailyNewsService;    
           this.mainPageService = mainPageService;  
              this._manageservice = manageservice;
              this.publicPageService = publicPageService;
            this._attachmentService = _attachmentService;
            this._deathservice = deathClaimService;

        }

        [HttpGet("getversion")]
        public async Task<CustomApiResponse> getversion()
        {
            var response = new CustomApiResponse();
            try
            {
                var version = "1.9";
                response.IsSucess = true;
                response.Value = version;
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


        [HttpGet]
        public async Task<CustomApiResponse> GetAll()
        {
            var response = new CustomApiResponse();
            try
            {
                var deathClaims = await _deathservice.GetAllAsync();
                response.IsSucess = true;
                response.Value = deathClaims;
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




        [HttpGet("mainpage")]
        public async Task<CustomApiResponse> GetAllMainPage()
        {
            var response = new CustomApiResponse();
            try
            {
                var mainPages = await mainPageService.GetAllAsync();
                response.IsSucess = true;
                response.Value = mainPages;
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

        [HttpGet("publicpage")]
        public async Task<CustomApiResponse> GetAllPublicPage()
        {
            var response = new CustomApiResponse();
            try
            {
                var mainPages = await publicPageService.GetAllAsync();
                response.IsSucess = true;
                response.Value = mainPages;
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


        [HttpGet("dayquotes")]
        public async Task<CustomApiResponse> GetAllDayQuote()
        {
            var response = new CustomApiResponse();
            try
            {
                var dayQuotes = await dayQuoteService.GetAllAsync();
                response.IsSucess = true;
                response.Value = dayQuotes;
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


        [HttpGet("dailynews")]
        public async Task<CustomApiResponse> GetAllDailyNews()
        {
            var response = new CustomApiResponse();
            try
            {
                var dailyNews = await dailyNewsService .GetAllAsync();
                response.IsSucess = true;
                response.Value = dailyNews;
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


        [HttpGet("managingCommitee")]
        public async Task<CustomApiResponse> GetAllManagingCommitee()
        {
            var response = new CustomApiResponse();
            try
            {
                var managingComitees = await _manageservice.GetAllAsync();
                response.IsSucess = true;
                response.Value = managingComitees;
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


        [HttpGet("attachment")]
        public async Task<CustomApiResponse> GetAllAttachment()
        {
            var response = new CustomApiResponse();
            try
            {
                var attachments = await _attachmentService.GetAllAsync ();

                attachments = attachments.Where(u => u.TableName == "public" && !u.IsDeleted).ToList();
                response.IsSucess = true;
                response.Value = attachments;
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

        [HttpGet("download/{attachmentId}")]
        public async Task<IActionResult> Download(int attachmentId)
        {
            var result = await _attachmentService.DownloadAttachmentAsync(attachmentId);

            if (result.ErrorMessage != null)
                return NotFound(ApiResponseFactory.Fail(result.ErrorMessage, System.Net.HttpStatusCode.NotFound));

            return File(result.FileStream!, result.ContentType!, result.FileName!);
        }

    }
}
