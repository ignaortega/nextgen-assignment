using PaySpace.Calculator.Common.Models;

namespace PaySpace.Calculator.Web.Services.Abstractions
{
    public interface ICalculatorHttpService
    {
        Task<List<PostalCodeDto>> GetPostalCodesAsync();

        Task<List<CalculatorHistoryDto>> GetHistoryAsync();

        Task<CalculateResult> CalculateTaxAsync(CalculateRequest calculationRequest);
        Task DeleteHistory(long id);
    }
}