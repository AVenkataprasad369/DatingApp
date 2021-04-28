using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using API.Interfaces;
using API.Services;
using API.Data;
using API.Helpers;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices
        (this IServiceCollection services, IConfiguration config )
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            services.AddDbContext<DataContext>(options =>
            { 
                //// connection string should come from appsettings development json file
                 //options.UseSqlite(_config.GetConnectionString("DefaultConnection"));
                //// For woring, I hard coded, but above code is correct one
                options.UseSqlite("Data Source=datingapp.db");
            });

            return services;
        }
    }
}