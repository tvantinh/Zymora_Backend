using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Zymora_BE.Middleware.ResponseModel;
using Zymora_BE.Middleware.IResponseModel;
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
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
            if (context.Response.StatusCode == (int)HttpStatusCode.OK && context.Response.ContentType == "application/json")
            {
                
            }
        }
        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var response = ResponseFactory.ResponseError<ValidationError>
            (
                context.Response.StatusCode,
                false,
                "An error occurred while processing your request.",
                new List<ValidationError>
                {
                    new ValidationError("Exception", ex.Message)
                },
                context.TraceIdentifier

            );
            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}