using System.Configuration;

using Microsoft.Extensions.Configuration;

using PaySpace.Calculator.Web.Services;

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();
var calculatorApiSettings = config.GetSection("CalculatorSettings");
if(string.IsNullOrEmpty(calculatorApiSettings.GetValue<string>("ApiUrl")))
{
    throw new ApplicationException("Missing API url");
}

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<CalculatorApiSettings>(calculatorApiSettings);
builder.Services.AddCalculatorHttpServices();
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(name: "default", pattern: "{controller=Calculator}/{action=Index}/{id?}");

app.Run();