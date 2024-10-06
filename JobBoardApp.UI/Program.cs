using JobBoardApp.Application.Mappings;
using JobBoardApp.Infrastructure.Configurations;
using JobBoardApp.Infrastructure.RealTime;
using JobBoardApp.Infrastructure.Services;
using JobBoardApp.UI.Services;
using JobBoardApp.UI.Services.IServices;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
    });


// configure database
builder.Services.AddDatabaseConfiguration(builder.Configuration);

// configure identity
builder.Services.AddIdentityConfiguration();


// configure lifetime for services
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddScoped<ITokenProvider, TokenProvider>();

builder.Services.AddSignalR();

// configure automapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// adding authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromHours(10);
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/AccessDenied";
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapAreaControllerRoute(
    name: "MyAreaEmployer",
    areaName: "Employer",
    pattern: "Employer/{controller=JobListing}/{action=Index}");

app.MapAreaControllerRoute(
    name: "MyAreaArchitect",
    areaName: "Architect",
    pattern: "Architect/{controller=JobListing}/{action=Index}");

app.MapAreaControllerRoute(
    name: "MyAreaJobSeeker",
    areaName: "JobSeeker",
    pattern: "JobSeeker/{controller=JobApplication}/{action=Index}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<NotificationHub>("/notificationHub");

app.SeedDatabase();

app.Run();
