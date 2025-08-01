using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Zymora_BE.Middleware.ResponseModel;
namespace Zymora_BE.Middleware
{
    public class ResponseWrappingMiddleware
    {
        private readonly RequestDelegate _next;
        public ResponseWrappingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (IsSwagger(context))
            {
                await _next(context);
                return;
            }
            var originalBodyStream = context.Response.Body;
           
                using (var responseBody = new MemoryStream())
                {
                try
                {
                    context.Response.Body = responseBody;
                    // Call the next middleware in the pipeline
                    await _next(context);
                    // Reset the body stream position to the beginning
                    responseBody.Seek(0, SeekOrigin.Begin);
                    // Read the response body
                    var responseContent = await new StreamReader(responseBody).ReadToEndAsync();
                    // Write the wrapped response back to the original body stream
                    context.Response.Body = originalBodyStream;
                    context.Response.ContentType = "application/json";
                    if (context.Response.StatusCode >= 200 && context.Response.StatusCode < 300)
                    {
                        await HandleSuccessRequestAsync(context, responseContent);
                    }
                    else if (context.Response.StatusCode >= 400 && context.Response.StatusCode < 500)
                    {
                        await HandleErrorRequestAsync(context, responseContent);
                    }
                    else
                    {
                        await HandleExceptionRequestAsync(context, new Exception("An unexpected error occurred."));
                    }
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    await HandleExceptionRequestAsync(context, ex);
                }
            }
            
        }
        private static Task HandleSuccessRequestAsync(HttpContext context, object data)
        {
            BaseModel<object> wrappedResponse = ResponseFactory.ResponseSuccess<object>(
                context.Response.StatusCode,
                true,
                "Request was successful",
                data);
            
            return context.Response.WriteAsJsonAsync(wrappedResponse);
        }
        private static Task HandleErrorRequestAsync(HttpContext context, object error)
        {
            BaseModel<object> wrappedResponse = ResponseFactory.ResponseError<object>(
                context.Response.StatusCode,
                false,
                "Bad request",
                error);
            
            return context.Response.WriteAsJsonAsync(wrappedResponse);
        }
        private static Task HandleExceptionRequestAsync(HttpContext context, Exception exception)
        {
            BaseModel<object> wrappedResponse = ResponseFactory.ResponseError<object>(
                context.Response.StatusCode,
                false,
                exception.Message,
                exception.Data);
            
            return context.Response.WriteAsJsonAsync(wrappedResponse);
        }
        private bool IsSwagger(HttpContext context)
        {
            return context.Request.Path.StartsWithSegments("/swagger");
        }
    }
}
