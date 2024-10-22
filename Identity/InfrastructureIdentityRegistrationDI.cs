using MuseWave.Application.Contracts.Identity;
using MuseWave.Identity.Models;
using MuseWave.Identity.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MuseWave.Identity
{
    public static class InfrastructureIdentityRegistrationDI
    {
        public static IServiceCollection AddInfrastrutureIdentityToDI(
                       this IServiceCollection services,
                                  IConfiguration configuration)
        {
            
            services.AddDbContext<ApplicationDbContext>(
               options =>
               options.UseSqlServer(
                   configuration.GetConnectionString
                   ("MuseWaveUserConnection"),
                   builder =>
                   builder.MigrationsAssembly(
                       typeof(ApplicationDbContext)
                       .Assembly.FullName)));

            // For Identity  
            services.AddIdentity<ApplicationUser, IdentityRole>()
                            .AddEntityFrameworkStores<ApplicationDbContext>()
                            .AddDefaultTokenProviders();
            // Adding Authentication  
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })

                        // Adding Jwt Bearer  
                        .AddJwtBearer(options =>
                        {
                            options.SaveToken = true;
                            options.RequireHttpsMetadata = false;
                            var configRoot = configuration as IConfigurationRoot;
                            if (configRoot != null)
                            {
                                Console.WriteLine(configRoot.GetDebugView());
                            }
                            var jwtSecret = configuration["SecretKey"];
                            if (string.IsNullOrEmpty(jwtSecret))
                            {
                                throw new ArgumentNullException(nameof(jwtSecret), "JWT:Secret configuration value is missing.");
                            }
                            Console.WriteLine($"JWT:Secret: {jwtSecret}");

                            options.TokenValidationParameters = new TokenValidationParameters()
                            {
                                ValidateIssuer = true,
                                ValidateAudience = true,
                                ValidAudience = configuration["JWT:ValidAudience"],
                                ValidIssuer = configuration["JWT:ValidIssuer"],
                                ClockSkew = TimeSpan.Zero,
                                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))                            };
                        });
            services.AddScoped
               <IAuthService, AuthService>();
            return services;
        }
        
    }
}