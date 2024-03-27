using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaySpace.Calculator.Web.Services.Abstractions;

namespace PaySpace.Calculator.Web.Services
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCalculatorHttpServices(this IServiceCollection services, IConfiguration calculatorApiSettings)
        {
            services.AddScoped<ICalculatorHttpService, CalculatorHttpService>();
            services.AddHttpClient<CalculatorHttpService>("CalculatorHttpClient", client =>
            {
                client.BaseAddress = new Uri(calculatorApiSettings.Get<CalculatorApiSettings>().ApiUrl);
            });
        }
    }
}