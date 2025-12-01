//using System.Net;
//using Cbeua.Domain.DTO;

//namespace Cbeua.InfraCore.External
//{
//    public class CustomResponseBuilder : ICustomResponseBuilder
//    {

//        public CustomApiResponse GetCustomApiResponse()
//        {
//            CustomApiResponse customApiResponse = new CustomApiResponse();
//            customApiResponse.IsSucess = false;
//            return customApiResponse;
//        }


//        public void SuccessReponse(object data, ref CustomApiResponse response, string? message = "")
//        {
//            response.IsSucess = true;
//            response.Value = data;
//            response.CustomMessage = message;
//            response.StatusCode = (int)HttpStatusCode.OK;


//        }
//        public void SuccessReponse(object data, ref CustomApiResponse response, int statuscode)
//        {
//            response.IsSucess = true;
//            response.Value = data;

//            response.StatusCode = (int)statuscode;



//        }

//        public void CustomErrorReponse(ref CustomApiResponse response, int? statuscode = 0,string?message="")
//        {
//            response.IsSucess = false;
//            response.CustomMessage = message;
//            if (statuscode == null || statuscode == 0)
//            {
//                response.StatusCode = (int)HttpStatusCode.BadRequest;
//            }
//            else
//            {
//                response.StatusCode = (int)statuscode;
//            }

//        }


//        public void ExceptionResponse(Exception ex, ref CustomApiResponse response)
//        {
//            response.StatusCode = (int)HttpStatusCode.BadRequest;
//            response.Error = ex.Message;
//            response.IsSucess = false;
//            response.Value = null;
//        }
//        public void ActionNotAuthorizedResponse(ref CustomApiResponse response)
//        {
//            response.StatusCode = (int)HttpStatusCode.Unauthorized;
//            response.CustomMessage = "Not Authorized for this action";
//            response.IsSucess = false;
//            response.Value = null;
//        }
//    }
//}