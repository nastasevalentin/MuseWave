using System.Reflection;
using IdentityServer4.AccessTokenValidation;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MuseWave.API.Services;
using MuseWave.Application;
using MuseWave.Application.Contracts.Interfaces;
using MuseWave.Application.Models;
using MuseWave.Identity;
using Microsoft.OpenApi.Models;
using MuseWave.Application.Features.Albums.Commands.CreateAlbum;
using MuseWave.Application.Features.Albums.Commands.DeleteAlbum;
using MuseWave.Application.Features.Albums.Commands.UpdateAlbum;
using MuseWave.Application.Features.Songs.Commands.CreateSong;
using MuseWave.Application.Features.Songs.Commands.DeleteSong;
using MuseWave.Application.Features.Songs.Commands.UpdateSong;
using MuseWave.Identity.Models;
using MuseWave.Identity.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

builder.Services.AddTransient<IRequestHandler<CreateSongCommand, CreateSongCommandResponse>, CreateSongCommandHandler>();
builder.Services.AddTransient<IRequestHandler<UpdateSongCommand, UpdateSongCommandResponse>, UpdateSongCommandHandler>();
builder.Services.AddTransient<IRequestHandler<DeleteSongCommand, DeleteSongCommandResponse>, DeleteSongCommandHandler>();
builder.Services.AddTransient<IRequestHandler<CreateAlbumCommand, CreateAlbumCommandResponse>, CreateAlbumCommandHandler>();
builder.Services.AddTransient<IRequestHandler<UpdateAlbumCommand, UpdateAlbumCommandResponse>, UpdateAlbumCommandHandler>();
builder.Services.AddTransient<IRequestHandler<DeleteAlbumCommand, DeleteAlbumCommandResponse>, DeleteAlbumCommandHandler>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddCors(options =>
{
    options.AddPolicy("Open", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddInfrastrutureToDI(
    builder.Configuration);
builder.Services.AddInfrastrutureIdentityToDI(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddControllers();
builder.Services.AddScoped<RoleService>();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,

            },
            new List<string>()
        }
    });

    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "MuseWave API", 

    });
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("Open");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();


public partial class Program
{

}
