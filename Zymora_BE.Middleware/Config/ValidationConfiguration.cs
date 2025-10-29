using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zymora_BE.Middleware.Handler;

namespace Zymora_BE.Middleware.Config
{
  public static class ValidationConfiguration
  {
    public static void ConfigureValidation(this IServiceCollection services)
    {
      services.Configure<ApiBehaviorOptions>(options =>
      {
        options.InvalidModelStateResponseFactory = context =>
        {
          return new ValidationProblemDetailsResult(context.ModelState);
        };
      });
    }
  }
}
