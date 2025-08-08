using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Zymora_BE.Middleware.IResponseModel;
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

            Stream originalBodyStream = context.Response.Body;

            await using MemoryStream responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            try
            {
                await _next(context);

                responseBody.Position = 0;
                string bodyText;
                using (StreamReader reader = new StreamReader(responseBody, leaveOpen: true))
                {
                    bodyText = await reader.ReadToEndAsync();
                }

                context.Response.Headers.ContentLength = null;
                responseBody.Position = 0;

                if (context.Response.StatusCode is >= 200 and < 300)
                {
                    context.Response.Body = originalBodyStream;
                    await HandleSuccessAsync(context, context.Response.StatusCode, bodyText);
                }
                else
                {
                    ValidationProblemDetails? pd = System.Text.Json.JsonSerializer.Deserialize<ValidationProblemDetails>(
                            bodyText,
                            new System.Text.Json.JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            }
                        );
                    IEnumerable<ValidationError> errors = pd?.Errors != null ? pd.Errors.SelectMany(kvp => kvp.Value.Select(msg => new ValidationError(Field: kvp.Key, message: msg))) : Enumerable.Empty<ValidationError>();


                    context.Response.Body = originalBodyStream;
                    string title = pd?.Title ?? Microsoft.AspNetCore.WebUtilities.ReasonPhrases.GetReasonPhrase(context.Response.StatusCode) ?? "Error";
                    
                    await HandleErrorAsync(context, context.Response.StatusCode,title,errors );
                }
            }
            catch (Exception ex)
            {
                context.Response.Body = originalBodyStream;
                await HandleExceptionAsync(context, ex);
            }
            finally
            {
                context.Response.Body = originalBodyStream;
            }
        }

        private static Task HandleSuccessAsync(HttpContext context, int statusCode, object data)
        {
            BaseModel<object> response = ResponseFactory.ResponseSuccess<object>(
                statusCode,
                true,
                "success",
                data,
                context.TraceIdentifier
            );
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsJsonAsync(response);
        }
        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            BaseModel<ValidationError> response = ResponseFactory.ResponseError<ValidationError>
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
            return context.Response.WriteAsJsonAsync(response);
        }
        private static Task HandleErrorAsync(HttpContext context,int Statuscode, string message, IEnumerable<ValidationError> errorValidations)
        {

            BaseModel<object> response = ResponseFactory.ResponseError<object>(
                statusCode: Statuscode,
                success: false,
                message: message,
                errors: errorValidations,
                TraceID: context.TraceIdentifier
            );

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = Statuscode;
            context.Response.Headers.ContentLength = null;

            return context.Response.WriteAsJsonAsync(response);
        }
        private bool IsSwagger(HttpContext context)
        {
            return context.Request.Path.StartsWithSegments("/swagger");
        }
    }
}