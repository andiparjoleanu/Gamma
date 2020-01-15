using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Gamma.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Gamma;
using Gamma.Models;
using Microsoft.AspNetCore.Identity;

namespace GammaAPI
{
    public static class ServicesExtensionMethods
    {
        public static void ConfigureSqlDbContext(this IServiceCollection services, IConfiguration config)
        {
            string connectionString = config.GetConnectionString("sqlConnectionString");
            services.AddDbContext<GammaContext>(server => server.UseSqlServer(connectionString, b => b.MigrationsAssembly("GammaAPI")));
        }

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<Member, IdentityRole>(options => {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<GammaContext>();
        }

        public static void ConfigureRepositoryWrapper(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
        }
    }
}
