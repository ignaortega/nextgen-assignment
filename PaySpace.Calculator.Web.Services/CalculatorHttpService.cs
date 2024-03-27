using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text;

using PaySpace.Calculator.Web.Services.Abstractions;
using PaySpace.Calculator.Web.Services.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PaySpace.Calculator.Web.Services
{
    public class CalculatorHttpService : ICalculatorHttpService
    {
        protected HttpClient _httpClient;
        public CalculatorHttpService(HttpClient httpClient) 
        {
            this._httpClient = httpClient;
            this._httpClient.BaseAddress = new Uri("https://localhost:7119");
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

        public async Task<List<CalculatorHistory>> GetHistoryAsync()
        {
            var response = await this._httpClient.GetAsync("api/history");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Cannot fetch tax history, status code: {response.StatusCode}");
            }

            return await response.Content.ReadFromJsonAsync<List<CalculatorHistory>>() ?? new List<CalculatorHistory>();
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
            var response = await this._httpClient.PostAsync($"api/delete-history?id={id}", null);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Cannot calculate tax, status code: {response.StatusCode}");
            }

            return;
        }
    }
}