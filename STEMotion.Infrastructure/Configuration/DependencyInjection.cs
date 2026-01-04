using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using STEMotion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Infrastructure.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfranstructureToApplication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<StemotionContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            return services;

        }
    }
}
