using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using qodeless.desafio.crosscutting.identity.Authorization;
using qodeless.desafio.crosscutting.identity.Entities;
using qodeless.desafio.crosscutting.identity.Repositories;
using qodeless.desafio.domain.Interfaces;
using qodeless.desafio.Infra.CrossCutting.identity.Data;

namespace qodeless.desafio.Infra.CrossCutting.Ioc
{
    public class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            //Add Authorization
            services.AddSingleton<IAuthorizationHandler, RoleRequerimentHandler>();

            // Infra - Identity
            services.AddScoped<IUser, AspNetUser>();

            // Data - Repository
            services.AddScoped<IUbuntuRepository, UbuntuRepository>();

            //Services
            //services.AddScoped<IUbuntuService, UbuntuService>();

            // DbContext - Services
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
            services.AddTransient<IAppDbContext, ApplicationDbContext>();
        }
    }
}
