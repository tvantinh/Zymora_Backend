using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Zymora_BE.Middleware.Middleware
{
  public class ResponseWrapperMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly ILogger<ResponseWrapperMiddleware> _logger;

    public ResponseWrapperMiddleware(RequestDelegate next, ILogger<ResponseWrapperMiddleware> logger)
    {
      _next = next;
      _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
      var originalBodyStream = context.Response.Body;

      using var responseBody = new MemoryStream();
      context.Response.Body = responseBody;

      try
      {
        await _next(context);

        if (ShouldWrapResponse(context))
        {
          await WrapResponse(context, originalBodyStream);
        }
        else
        {
          await CopyResponseBody(context, originalBodyStream);
        }
      }
      catch (Exception ex)
      {
        await HandleExceptionAsync(context, ex, originalBodyStream);
      }
    }

    private bool ShouldWrapResponse(HttpContext context)
    {
      var path = context.Request.Path.Value?.ToLower() ?? "";

      if (path.StartsWith("/swagger") ||
          path.StartsWith("/health") ||
          path.StartsWith("/static") ||
          path.Contains("."))
      {
        return false;
      }

      var contentType = context.Response.ContentType?.ToLower() ?? "";
      if (!contentType.Contains("application/json") && !string.IsNullOrEmpty(contentType))
      {
        return false;
      }

      return true;
    }

    private async Task WrapResponse(HttpContext context, Stream originalBodyStream)
    {
      context.Response.Body.Seek(0, SeekOrigin.Begin);
      var responseBodyText = await new StreamReader(context.Response.Body).ReadToEndAsync();

      object wrappedResponse;

      if (context.Response.StatusCode >= 200 && context.Response.StatusCode < 300)
      {
        object? data = null;

        if (!string.IsNullOrEmpty(responseBodyText))
        {
          try
          {
            data = JsonSerializer.Deserialize<object>(responseBodyText);
          }
          catch
          {
            data = responseBodyText;
          }
        }

        wrappedResponse = new
        {
          success = true,
          message = GetSuccessMessage(context.Response.StatusCode),
          data = data,
          errors = new Dictionary<string, List<string>>(),
          timestamp = DateTime.UtcNow,
          path = context.Request.Path.Value,
          traceId = context.TraceIdentifier
        };
      }
      else
      {
        var errors = new Dictionary<string, List<string>>();

        if (!string.IsNullOrEmpty(responseBodyText))
        {
          try
          {
            var errorObj = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBodyText);
            if (errorObj != null && errorObj.ContainsKey("errors"))
            {
              var errorsJson = JsonSerializer.Serialize(errorObj["errors"]);
              errors = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(errorsJson) ?? new Dictionary<string, List<string>>();
            }
            else
            {
              errors = new Dictionary<string, List<string>>
              {
                { "general", new List<string> { responseBodyText } }
              };
            }
          }
          catch
          {
            errors = new Dictionary<string, List<string>>
            {
              { "general", new List<string> { responseBodyText } }
            };
          }
        }

        wrappedResponse = new
        {
          success = false,
          status = context.Response.StatusCode,
          message = GetErrorMessage(context.Response.StatusCode),
          data = (object?)null,
          errors = errors,
          timestamp = DateTime.UtcNow,
          path = context.Request.Path.Value,
          traceId = context.TraceIdentifier
        };
      }

      var wrappedJson = JsonSerializer.Serialize(wrappedResponse, new JsonSerializerOptions
      {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
      });

      context.Response.ContentType = "application/json";
      context.Response.ContentLength = null;

      await originalBodyStream.WriteAsync(System.Text.Encoding.UTF8.GetBytes(wrappedJson));
    }

    private async Task CopyResponseBody(HttpContext context, Stream originalBodyStream)
    {
      context.Response.Body.Seek(0, SeekOrigin.Begin);
      await context.Response.Body.CopyToAsync(originalBodyStream);
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception, Stream originalBodyStream)
    {
      _logger.LogError(exception, "Unhandled exception occurred");

      var statusCode = exception switch
      {
        ArgumentException => HttpStatusCode.BadRequest,
        UnauthorizedAccessException => HttpStatusCode.Unauthorized,
        KeyNotFoundException => HttpStatusCode.NotFound,
        _ => HttpStatusCode.InternalServerError
      };

      var errors = new Dictionary<string, List<string>>
      {
        ["exception"] = new List<string> { exception.Message }
      };

      if (exception.InnerException != null)
      {
        errors["innerException"] = new List<string> { exception.InnerException.Message };
      }

      var response = new
      {
        success = false,
        message = "An error occurred processing your request",
        data = (object?)null,
        errors = errors,
        timestamp = DateTime.UtcNow,
        path = context.Request.Path.Value,
        traceId = context.TraceIdentifier
      };

      var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
      {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
      });

      context.Response.Clear();
      context.Response.StatusCode = (int)statusCode;
      context.Response.ContentType = "application/json";

      await originalBodyStream.WriteAsync(System.Text.Encoding.UTF8.GetBytes(json));
    }

    private string GetSuccessMessage(int statusCode)
    {
      return statusCode switch
      {
        200 => "Success",
        201 => "Created successfully",
        204 => "No content",
        _ => "Request completed successfully"
      };
    }

    private string GetErrorMessage(int statusCode)
    {
      return statusCode switch
      {
        400 => "Bad request",
        401 => "Unauthorized",
        403 => "Forbidden",
        404 => "Not found",
        409 => "Conflict",
        422 => "Unprocessable entity",
        500 => "Internal server error",
        _ => "An error occurred"
      };
    }
  }
}
