using PaySpace.Calculator.Data.Models;
using PaySpace.Calculator.Services.Abstractions;
using PaySpace.Calculator.Services.Exceptions;

namespace PaySpace.Calculator.Services.Calculators
{
    internal sealed class FlatRateCalculator : BaseCalculator, ICalculator, IFlatRateCalculator
    {
        public FlatRateCalculator(ICalculatorSettingsService calculatorSettingsService) : base(calculatorSettingsService)
        {
            this._calculatorType = CalculatorType.FlatRate;
        }

        protected override decimal CalculateTax(List<CalculatorSetting> calculatorSettings, decimal income)
        {
            if (calculatorSettings.Count > 1)
                throw new CalculatorException($"Expected only one rate, got {calculatorSettings.Count}");

            var calculatorSetting = calculatorSettings.First();
            return income * calculatorSetting.Rate / 100;
        }
    }
}