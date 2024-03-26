using System.Runtime.InteropServices.Marshalling;

using PaySpace.Calculator.Data.Models;
using PaySpace.Calculator.Services.Abstractions;
using PaySpace.Calculator.Services.Exceptions;
using PaySpace.Calculator.Services.Models;

namespace PaySpace.Calculator.Services.Calculators
{
    public abstract class BaseCalculator : ICalculator
    {
        protected ICalculatorSettingsService _calculatorSettingsService;
        protected CalculatorType _calculatorType;

        public BaseCalculator(ICalculatorSettingsService calculatorSettingsService)
        {
            this._calculatorSettingsService = calculatorSettingsService;
        }

        public async Task<CalculateResult> CalculateAsync(decimal income)
        {
            return CalculateResult(income);
        }

        private CalculateResult CalculateResult(decimal income)
        {
            var calculatorSettings = this._calculatorSettingsService.GetSettingsAsync(this._calculatorType).Result;

            if (calculatorSettings == null || !calculatorSettings.Any())
            {
                throw new CalculatorException($"There are no settings for Calculator {this._calculatorType.ToString()}");
            }

            return new CalculateResult()
            {
                Calculator = this._calculatorType,
                Tax = income > 0 ? CalculateTax(calculatorSettings, income) : 0
            };
        }

        protected abstract decimal CalculateTax(List<CalculatorSetting> calculatorSettings, decimal income);
    }
}
