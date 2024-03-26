using PaySpace.Calculator.Services.Models;

namespace PaySpace.Calculator.Services.Abstractions
{
    internal interface ICalculator
    {
        Task<CalculateResult> CalculateAsync(decimal income);
    }
}
