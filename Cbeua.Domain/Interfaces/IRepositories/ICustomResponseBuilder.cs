using Cbeua.Domain.DTO;

namespace Cbeua.InfraCore.External
{
    public interface ICustomResponseBuilder
    {
        void ActionNotAuthorizedResponse(ref CustomApiResponse response);
        void CustomErrorReponse(ref CustomApiResponse response, int? statuscode = 0,string? message="");
        void ExceptionResponse(Exception ex, ref CustomApiResponse response);
        CustomApiResponse GetCustomApiResponse();
        void SuccessReponse(object data, ref CustomApiResponse response, string? message = "");
        void SuccessReponse(object data, ref CustomApiResponse response, int statuscode);
    }
}