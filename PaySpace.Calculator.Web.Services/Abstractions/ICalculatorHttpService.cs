using PaySpace.Calculator.Web.Services.Models;

namespace PaySpace.Calculator.Web.Services.Abstractions
{
    public interface ICalculatorHttpService
    {
        Task<List<PostalCodeDto>> GetPostalCodesAsync();

        Task<List<CalculatorHistory>> GetHistoryAsync();

        Task<CalculateResult> CalculateTaxAsync(CalculateRequest calculationRequest);
        Task DeleteHistory(long id);
    }
}