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
            // Assumption:
            // The provided rate ranges for the progressive tax have a 1 unit gap
            // Taking into account the expected results in the already defined test
            // I'm assuming that any value falling into that gap
            // would be calculated using the rate of the lower end of the gap
            // In a real life situation, I'd double check with business the desired behavior
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