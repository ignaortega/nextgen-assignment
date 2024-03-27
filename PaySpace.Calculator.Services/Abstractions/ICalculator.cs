using PaySpace.Calculator.Common.Models;

namespace PaySpace.Calculator.Services.Abstractions
{
    public interface ICalculator
    {
        Task<CalculateResult> CalculateAsync(decimal income);
    }
}
