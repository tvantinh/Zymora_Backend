using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zymora_BE.Middleware.Handler
{
  public static class ValidationErrorHandler
  {
    public static Dictionary<string, List<string>> GetValidationErrors(ModelStateDictionary modelState)
    {
      var errors = new Dictionary<string, List<string>>();

      foreach (var key in modelState.Keys)
      {
        var state = modelState[key];
        if (state != null && state.Errors != null && state.Errors.Count > 0)
        {
          var errorMessages = state.Errors
              .Select(error => string.IsNullOrEmpty(error.ErrorMessage)
                  ? error.Exception?.Message ?? "Invalid value"
                  : error.ErrorMessage)
              .ToList();

          errors.Add(ToCamelCase(key), errorMessages);
        }
      }

      return errors;
    }

    private static string ToCamelCase(string str)
    {
      if (string.IsNullOrEmpty(str) || char.IsLower(str[0]))
        return str;

      return char.ToLower(str[0]) + str.Substring(1);
    }
  }
}
