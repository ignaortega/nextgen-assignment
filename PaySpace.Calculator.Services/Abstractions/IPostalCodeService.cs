using PaySpace.Calculator.Common.Models;
using PaySpace.Calculator.Data.Models;

namespace PaySpace.Calculator.Services.Abstractions
{
    public interface IPostalCodeService
    {
        Task<List<PostalCode>> GetPostalCodesAsync();

        Task<CalculatorType?> CalculatorTypeAsync(string code);
    }
}