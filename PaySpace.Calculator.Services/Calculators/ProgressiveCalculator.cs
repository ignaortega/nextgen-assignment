using PaySpace.Calculator.Common.Models;
using PaySpace.Calculator.Data.Models;
using PaySpace.Calculator.Services.Abstractions;

namespace PaySpace.Calculator.Services.Calculators
{
    internal sealed class ProgressiveCalculator : BaseCalculator, ICalculator, IProgressiveCalculator
    {
        public ProgressiveCalculator(ICalculatorSettingsService calculatorSettingsService) : base(calculatorSettingsService)
        {
            this._calculatorType = CalculatorType.Progressive;
        }

        protected override decimal CalculateTax(List<CalculatorSetting> calculatorSettings, decimal income)
        {
            decimal totalTax = 0;

            foreach (CalculatorSetting setting in calculatorSettings)
            {

                decimal taxableAmount = 0;

                if (setting.To.HasValue && Math.Truncate(income) > setting.To)
                {
                    taxableAmount = setting.To.Value - setting.From;
                }
                else if (setting.To.HasValue)
                {
                    taxableAmount = income - setting.From;
                }
                else
                {
                    taxableAmount = income - setting.From;
                }

                totalTax += taxableAmount * setting.Rate / 100;

                if (setting.To.HasValue && Math.Truncate(income) <= setting.To)
                {
                    break;
                }
            }

            return totalTax;
        }
    }
}