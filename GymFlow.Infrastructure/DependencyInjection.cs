using GymFlow.Application.Services;
using GymFlow.Infrastructure.Data;
using GymFlow.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {

            services.AddScoped<IMemberService, MemberService>();
            services.AddScoped<ISubscriptionTypeService, SubscriptionTypeService>();


            services.AddScoped<IAppDbContext>(provider => provider.GetService<AppDbContext>());

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")).LogTo(Console.WriteLine, LogLevel.Debug);
            });

            return services;
        }
    }
}
