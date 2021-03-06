using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using API.Interfaces;
using API.Services;
using API.Data;
using API.Helpers;
using API.SignalR;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices
        (this IServiceCollection services, IConfiguration config )
        {
            services.AddSingleton<PresenceTracker>();
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPhotoService, PhotoService>();
            // services.AddScoped<ILikesRepository, LikesRepository>(); // before UnitOfWork
            // services.AddScoped<IMessageRepository, MessageRepository>(); // before UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<LogUserActivity>();
            // services.AddScoped<IUserRepository, UserRepository>(); // before UnitOfWork
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