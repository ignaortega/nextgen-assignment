using System.Net.Http.Json;
using System.Text;

using Microsoft.Extensions.Configuration;

using Newtonsoft.Json;

using PaySpace.Calculator.Common.Models;
using PaySpace.Calculator.Web.Services.Abstractions;

namespace PaySpace.Calculator.Web.Services
{
    public class CalculatorHttpService : ICalculatorHttpService
    {
        protected HttpClient _httpClient;
        public CalculatorHttpService(HttpClient httpClient, IConfiguration configuration) 
        {
            var baseUrl = configuration.GetValue<string>("CalculatorSettings:ApiUrl");
            
            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new InvalidOperationException("Base URL is not configured.");
            }

            this._httpClient = httpClient;
            this._httpClient.BaseAddress = new Uri(baseUrl);
        }
        public async Task<List<PostalCodeDto>> GetPostalCodesAsync()
        {
            var response = await this._httpClient.GetAsync("api/postalcode");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Cannot fetch postal codes, status code: {response.StatusCode}");
            }

            return await response.Content.ReadFromJsonAsync<List<PostalCodeDto>>() ?? new List<PostalCodeDto>();
        }

        public async Task<List<CalculatorHistoryDto>> GetHistoryAsync()
        {
            var response = await this._httpClient.GetAsync("api/history");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Cannot fetch tax history, status code: {response.StatusCode}");
            }

            return await response.Content.ReadFromJsonAsync<List<CalculatorHistoryDto>>() ?? new List<CalculatorHistoryDto>();
        }

        public async Task<CalculateResult> CalculateTaxAsync(CalculateRequest calculationRequest)
        {
            string json = JsonConvert.SerializeObject(calculationRequest);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await this._httpClient.PostAsync("api/calculate-tax", httpContent);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Cannot calculate tax, status code: {response.StatusCode}");
            }

            return new CalculateResult();
        }

        public async Task DeleteHistory(long id)
        {
            HttpContent httpContent = new StringContent(string.Empty);
            var response = await this._httpClient.PostAsync("api/delete-history?id={id}", null);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Cannot calculate tax, status code: {response.StatusCode}");
            }

            return;
        }
    }
}