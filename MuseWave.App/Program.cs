using Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MuseWave.App.Pages;
using MuseWave.Identity;
using MuseWave.Identity.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(options=>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MuseWaveUserConnection")));
builder.Services.AddInfrastrutureToDI(builder.Configuration);
builder.Services.AddHttpClient(nameof(UserModel), client =>
    {
        client.BaseAddress = new Uri("https://localhost:7165/");
    })
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
    });
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();                                                
// Add services to the container.                                                                               
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();
builder.Services.AddAuthentication();                
// builder.Services.AddHttpClient<UserModel>(client => {
//     client.BaseAddress = new Uri("https://localhost:7165/");
// }).ConfigurePrimaryHttpMessageHandler(() =>
// {
//     return new HttpClientHandler
//     {
//         ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
//     };
// });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();
app.MapRazorPages();

app.Run();