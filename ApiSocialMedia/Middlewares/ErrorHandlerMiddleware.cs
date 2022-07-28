using Application.Wrappers;
using System.Net;
using System.Text.Json;

namespace ApiSocialMedia.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                var responseModel = new Response<string>()
                {
                    Success = false,
                    Message = error.Message
                };

                switch (error)
                {
                    // custom application error
                    case Application.Exceptions.ApiException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;

                    // custom application error
                    case Application.Exceptions.ValidationException e:
                        response.StatusCode= (int)HttpStatusCode.BadRequest;
                        responseModel.Errors = e.Errors;
                        break;

                    // not found error
                    case KeyNotFoundException e:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;

                    // unhandled error
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var result = JsonSerializer.Serialize(responseModel);

                await response.WriteAsync(result);
            }
        }
    }
}
