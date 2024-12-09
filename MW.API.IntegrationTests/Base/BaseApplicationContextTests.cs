using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace MW.API.IntegrationTests.Base;

public class BaseApplicationContextTests : IAsyncDisposable
{
    protected readonly WebApplicationFactory<Program> Application;
    protected readonly HttpClient Client;

    protected BaseApplicationContextTests()
    {
        Application = new WebApplicationFactory<Program>();
        Application = Application.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // var dbContextService = services.FirstOrDefault(service => service.ServiceType == typeof(DbContextOptions<GlobalMWContext>));
                // Console.WriteLine($"DbContextOptions<GlobalMWContext> service before removal: {dbContextService}");
                //
                // services.RemoveAll<DbContextOptions>();
                // services.RemoveAll(typeof(DbContextOptions<>));
                // Console.WriteLine("DbContextOptions removed");
                // dbContextService = services.FirstOrDefault(service => service.ServiceType == typeof(DbContextOptions<GlobalMWContext>));
                // Console.WriteLine($"DbContextOptions<GlobalMWContext> service after removal: {dbContextService}");
                //
                // services.AddDbContext<GlobalMWContext>(options =>
                // {
                //     options.UseInMemoryDatabase("GlobalMWDbForTesting");
                // });

                services.Configure<JwtBearerOptions>(
                    JwtBearerDefaults.AuthenticationScheme,
                    options =>
                    {
                        options.Configuration = new OpenIdConnectConfiguration
                        {
                            Issuer = JwtTokenProvider.Issuer,
                        };
                        options.TokenValidationParameters.ValidIssuer = JwtTokenProvider.Issuer;
                        options.TokenValidationParameters.ValidAudience = JwtTokenProvider.Issuer;
                        options.Configuration.SigningKeys.Add(JwtTokenProvider.SecurityKey);
                    }
                );

                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<GlobalMWContext>();
                    var dbContext = scope.ServiceProvider.GetRequiredService<GlobalMWContext>();
                    var dbProvider = dbContext.Database.ProviderName;
                    Console.WriteLine($"Database Provider: {dbProvider}");
                    if (db.Database.IsInMemory())
                    {
                        db.Database.EnsureDeleted();
                        db.Database.EnsureCreated();
                        Seed.InitializeDbForTests(db);
                    }
                }
            });
        });
        
        Client = Application.CreateClient();
    }

    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        await Application.DisposeAsync();
    }
}
