using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Cbeua.Domain.DTO;

namespace Cbeua.Core.Helpers
{
    public static class ApiResponseFactory
    {
        public static CustomApiResponse Success(object? data = null, string message = "", HttpStatusCode code = HttpStatusCode.OK)
            => new()
            {
                IsSucess = true,
                StatusCode = (int)code,
                Value = data,
                CustomMessage = message

            };

        public static CustomApiResponse Fail(string message = "", HttpStatusCode code = HttpStatusCode.BadRequest, string? error = null)
            => new()
            {
                IsSucess = false,
                StatusCode = (int)code,
                CustomMessage = message,
                Error = error
            };

        public static CustomApiResponse Unauthorized(string message = "Not authorized for this action")
            => Fail(message, HttpStatusCode.Unauthorized);

        public static CustomApiResponse Exception(Exception ex)
            => Fail("An unexpected error occurred.", HttpStatusCode.InternalServerError, ex.Message);
    }
}
