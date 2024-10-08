using MuseWave.Application.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Repositories;
using Infrastructure;

namespace Infrastructure
{
    public static class InfrastructureRegistrationDI
    {
        public static IServiceCollection AddInfrastrutureToDI(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<GlobalMWContext>(
                options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString
                            ("MuseWaveConnection"),
                        builder =>
                            builder.MigrationsAssembly(
                                typeof(GlobalMWContext)
                                    .Assembly.FullName)));
            services.AddScoped
            (typeof(IAsyncRepository<>),
                typeof(BaseRepository<>));
            services.AddScoped<
                ISongRepository, SongRepository>();
            services.AddScoped<
                IAlbumRepository, AlbumRepository>();
            return services;
        }
    }
}