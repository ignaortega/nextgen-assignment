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

            decimal tax = 0;

            if (calculatorSettings.Any() && income > 0)
            {
                tax = CalculateTax(calculatorSettings, income);
            }

            return new CalculateResult()
            {
                Calculator = this._calculatorType,
                Tax = tax
            };
        }

        protected abstract decimal CalculateTax(List<CalculatorSetting> calculatorSettings, decimal income);
    }
}
