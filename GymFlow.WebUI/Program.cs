using Microsoft.AspNetCore.Localization;
using System.Globalization;
using GymFlow.Infrastructure;
using GymFlow.Application.Services;
using GymFlow.Domain.DTOs.File;
using GymFlow.Infrastructure.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add Localization
//builder.Services.AddLocalization(options =>
//{
//    options.ResourcesPath = "Resources";
//});
builder.Services.AddLocalization();

// Configure localization
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("en"),
        new CultureInfo("ar"),
    };

    options.DefaultRequestCulture = new RequestCulture("en");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;

    options.ApplyCurrentCultureToResponseHeaders = true;

    options.RequestCultureProviders = new List<IRequestCultureProvider>
    {
        new QueryStringRequestCultureProvider(),
        new CookieRequestCultureProvider(),
        new AcceptLanguageHeaderRequestCultureProvider()
    };

});


var uploadsPath = Path.Combine(builder.Environment.WebRootPath, "uploads");

builder.Services.Configure<FileValidationOptions>(
    builder.Configuration.GetSection("FileValidation"));


builder.Services.AddScoped<IFileService>(x =>
{
    var logger = x.GetRequiredService<ILogger<FileService>>();

    var options = x.GetRequiredService<IOptions<FileValidationOptions>>()
    .Value;

    return new FileService(uploadsPath, options, logger);
});


builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();


// Configure localization middleware
var localizationOptions = app.Services
    .GetService<Microsoft.Extensions.Options
    .IOptions<RequestLocalizationOptions>>()?.Value;

app.UseRequestLocalization(localizationOptions);

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
