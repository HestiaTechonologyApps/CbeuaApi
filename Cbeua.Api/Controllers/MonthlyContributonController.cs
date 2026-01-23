//using Cbeua.Domain.DTO;
//using Cbeua.Domain.Interfaces.IServices;
//using Cbeua.Domain.Interfaces.IServices.Common;
//using Microsoft.AspNetCore.Mvc;

//namespace Cbeua.Api.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class MonthlyContributonController : Controller
//    {
//        private readonly IMonthlyContributionService  _service;
//        public MonthlyContributonController(IMonthlyContributionService service)
//        {
//            _service = service;
//        }

//        [HttpGet]
//        public IActionResult Index()
//        {
//            return View();
//        }

//        [HttpGet("getall-monthlyContribution-indiviadal")]
//        public async Task<CustomApiResponse> GetMonthlyContributionofIndividual(int MemberId=0)
//        {

//            var response = new CustomApiResponse();
//            try
//            {
//               var contr = await _service.GetIndividualContrbution(MemberId);
//                response.IsSucess = true;
//              //  response.Value = companys;
//                response.StatusCode = 200;
//            }
//            catch (Exception ex)
//            {
//                response.IsSucess = false;
//                response.Error = ex.Message;
//                response.StatusCode = 500;
//            }
//            return response;
//        }
//    }
//}
