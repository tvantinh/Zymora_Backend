using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zymora_BE.Middleware.Handler
{
  public class ValidationProblemDetailsResult : IActionResult
  {
    private readonly Dictionary<string, List<string>> _errors;

    public ValidationProblemDetailsResult(ModelStateDictionary modelState)
    {
      _errors = ValidationErrorHandler.GetValidationErrors(modelState);
    }

    public async Task ExecuteResultAsync(ActionContext context)
    {
      var response = new
      {
        success = false,
        message = "Validation failed",
        data = (object?)null,
        errors = _errors,
        timestamp = DateTime.UtcNow,
        path = context.HttpContext.Request.Path.Value,
        traceId = context.HttpContext.TraceIdentifier
      };

      context.HttpContext.Response.StatusCode = 400;
      context.HttpContext.Response.ContentType = "application/json";

      await context.HttpContext.Response.WriteAsJsonAsync(response);
    }
  }
}
