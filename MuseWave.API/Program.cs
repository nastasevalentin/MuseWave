﻿using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MuseWave.API.Services;
using MuseWave.API.Utility;
using MuseWave.Application;
using MuseWave.Application.Contracts.Interfaces;
using MuseWave.Application.Models;
using MuseWave.Identity;
using Microsoft.OpenApi.Models;
using MuseWave.Identity.Models;
using MuseWave.Identity.Services;




var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

builder.Services.AddHttpContextAccessor();
builder.Services.AddCors(options =>
{
    options.AddPolicy("Open", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
// Add services to the container.
builder.Services.AddInfrastrutureToDI(
    builder.Configuration);
builder.Services.AddInfrastrutureIdentityToDI(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddControllers();
builder.Services.AddScoped<RoleService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

    c.OperationFilter<FileResultContentTypeOperationFilter>();
});


var app = builder.Build();
// using (var scope = app.Services.CreateScope())
// {
//     var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
//     if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
//     {
//         await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
//     }
//
//     var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
//     var roleService = scope.ServiceProvider.GetRequiredService<RoleService>();
//
//     var user = await userManager.FindByNameAsync("valibi80");
//     if (user != null)
//     {
//         var result = await roleService.AssignRoleToUserAsync(user, UserRoles.Admin);
//         if (result)
//         {
//             Console.WriteLine("User 'valibi80' has been assigned the 'Admin' role.");
//         }
//         else
//         {
//             Console.WriteLine("Failed to assign the 'Admin' role to user 'valibi80'.");
//         }
//     }
//     else
//     {
//         Console.WriteLine("User 'valibi80' not found.");
//     }
// }
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("Open");
app.UseAuthorization();

app.MapControllers();

app.Run();