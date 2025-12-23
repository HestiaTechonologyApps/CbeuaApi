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
       
        public PublicController(IDailyNewsService dailyNewsService ,IDayQuoteService dayQuoteService ,IMainPageService mainPageService )
        {
            dayQuoteService = dayQuoteService;
            dailyNewsService = dailyNewsService;    
            mainPageService = mainPageService;  
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
    }
}
