using Azure.Core;
using Zymora_BE.Repositories.UnitOfWork;
using Zymora_BE.Contract.Repositories.IUnitOfWork;
using Zymora_BE.Contract.Services.IService;
using Zymora_BE.Services.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Zymora_BE.Services
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRespositories();
            services.AddServices();
        }
        public static void AddRespositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
        }
    }
}
