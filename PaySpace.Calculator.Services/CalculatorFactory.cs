using Microsoft.Extensions.DependencyInjection;

using PaySpace.Calculator.Common.Models;
using PaySpace.Calculator.Services.Abstractions;
using PaySpace.Calculator.Services.Exceptions;

namespace PaySpace.Calculator.Services
{
    public class CalculatorFactory : ICalculatorFactory
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly IPostalCodeService _postalCodeService;


        public CalculatorFactory(IServiceProvider serviceProvider, IPostalCodeService postalCodeService)
        {
            _serviceProvider = serviceProvider;
            _postalCodeService = postalCodeService;
        }
        public async Task<ICalculator> GetCalculatorAsync(string postalCode)
        {
            var calculatorType = await _postalCodeService.CalculatorTypeAsync(postalCode);

            if(!calculatorType.HasValue)
            {
                throw new CalculatorException($"Not suitable Calculator for postal code {postalCode}");
            }

            switch (calculatorType)
            {
                case CalculatorType.FlatRate:
                    return _serviceProvider.GetRequiredService<IFlatRateCalculator>();
                case CalculatorType.FlatValue:
                    return _serviceProvider.GetRequiredService<IFlatValueCalculator>();
                case CalculatorType.Progressive:
                    return _serviceProvider.GetRequiredService<IProgressiveCalculator>();
                default:
                    throw new CalculatorException($"Unkown calculator type {calculatorType.ToString()}");
            }
        }
    }
}
