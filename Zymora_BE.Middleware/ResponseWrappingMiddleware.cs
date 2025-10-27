using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Zymora_BE.Middleware.IResponseModel;
using Zymora_BE.Middleware.ResponseModel;

namespace Zymora_BE.Middleware
{
  public class ResponseWrappingMiddleware
  {
    private readonly RequestDelegate _next;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
      PropertyNameCaseInsensitive = true
    };

    public ResponseWrappingMiddleware(RequestDelegate next)
    {
      _next = next ?? throw new ArgumentNullException(nameof(next));
    }

    public async Task InvokeAsync(HttpContext context)
    {
      if (ShouldSkipWrapping(context))
      {
        await _next(context);
        return;
      }

      await ProcessResponseAsync(context);
    }

    private async Task ProcessResponseAsync(HttpContext context)
    {
      var originalBodyStream = context.Response.Body;

      await using var responseBody = new MemoryStream();
      context.Response.Body = responseBody;

      try
      {
        await _next(context);

        var bodyText = await ReadResponseBodyAsync(responseBody);
        context.Response.Headers.ContentLength = null;
        context.Response.Body = originalBodyStream;

        if (IsSuccessStatusCode(context.Response.StatusCode))
        {
          await WriteSuccessResponseAsync(context, bodyText);
        }
        else
        {
          await WriteErrorResponseAsync(context, bodyText);
        }
      }
      catch (Exception ex)
      {
        context.Response.Body = originalBodyStream;
        await WriteExceptionResponseAsync(context, ex);
      }
    }

    private static async Task<string> ReadResponseBodyAsync(MemoryStream responseBody)
    {
      responseBody.Position = 0;
      using var reader = new StreamReader(responseBody, leaveOpen: true);
      return await reader.ReadToEndAsync();
    }

    private static bool IsSuccessStatusCode(int statusCode) => statusCode is >= 200 and < 300;

    private static async Task WriteSuccessResponseAsync(HttpContext context, string bodyText)
    {
      var response = ResponseFactory.ResponseSuccess(
          statusCode: context.Response.StatusCode,
          success: true,
          message: "success",
          data: bodyText,
          TraceId: context.TraceIdentifier
      );

      await WriteJsonResponseAsync(context, response, context.Response.StatusCode);
    }

    private static async Task WriteErrorResponseAsync(HttpContext context, string bodyText)
    {
      var validationErrors = ExtractValidationErrors(bodyText);
      var title = ExtractErrorTitle(bodyText, context.Response.StatusCode);

      var response = ResponseFactory.ResponseError<object>(
          statusCode: context.Response.StatusCode,
          success: false,
          message: title,
          errors: validationErrors,
          TraceID: context.TraceIdentifier
      );

      await WriteJsonResponseAsync(context, response, context.Response.StatusCode);
    }

    private static async Task WriteExceptionResponseAsync(HttpContext context, Exception ex)
    {
      var response = ResponseFactory.ResponseError<ValidationError>(
          statusCode: (int)HttpStatusCode.InternalServerError,
          success: false,
          message: "An error occurred while processing your request.",
          errors: new List<ValidationError>
          {
                    new ValidationError("Exception", ex.Message)
          },
          TraceID: context.TraceIdentifier
      );

      await WriteJsonResponseAsync(context, response, (int)HttpStatusCode.InternalServerError);
    }

    private static IEnumerable<ValidationError> ExtractValidationErrors(string bodyText)
    {
      try
      {
        var problemDetails = JsonSerializer.Deserialize<ValidationProblemDetails>(bodyText, JsonOptions);

        return problemDetails?.Errors?
            .SelectMany(kvp => kvp.Value.Select(msg => new ValidationError(Field: kvp.Key, message: msg)))
            ?? Enumerable.Empty<ValidationError>();
      }
      catch
      {
        return Enumerable.Empty<ValidationError>();
      }
    }

    private static string ExtractErrorTitle(string bodyText, int statusCode)
    {
      try
      {
        var problemDetails = JsonSerializer.Deserialize<ValidationProblemDetails>(bodyText, JsonOptions);
        return problemDetails?.Title
            ?? ReasonPhrases.GetReasonPhrase(statusCode)
            ?? "Error";
      }
      catch
      {
        return ReasonPhrases.GetReasonPhrase(statusCode) ?? "Error";
      }
    }

    private static async Task WriteJsonResponseAsync<T>(HttpContext context, T response, int statusCode)
    {
      context.Response.ContentType = "application/json";
      context.Response.StatusCode = statusCode;
      context.Response.Headers.ContentLength = null;
      await context.Response.WriteAsJsonAsync(response);
    }

    private static bool ShouldSkipWrapping(HttpContext context)
    {
      return context.Request.Path.StartsWithSegments("/swagger");
    }
  }
}