using PaySpace.Calculator.Common.Models;
using PaySpace.Calculator.Data.Models;
using PaySpace.Calculator.Services.Abstractions;

namespace PaySpace.Calculator.Services.Calculators
{
    internal sealed class FlatValueCalculator : BaseCalculator, ICalculator, IFlatValueCalculator
    {
        public FlatValueCalculator(ICalculatorSettingsService calculatorSettingsService) : base(calculatorSettingsService)
        {
            this._calculatorType = CalculatorType.FlatValue;
        }

        protected override decimal CalculateTax(List<CalculatorSetting> calculatorSettings, decimal income)
        {
            decimal tax = 0;
            var settingForIncome = calculatorSettings.Where(s => s.From <= income && (!s.To.HasValue || s.To >= income)).FirstOrDefault();
            if (settingForIncome != null)
            {
                switch (settingForIncome.RateType)
                {
                    case RateType.Percentage:
                        tax = income * settingForIncome.Rate / 100;
                        break;
                    case RateType.Amount:
                        tax = settingForIncome.Rate;
                        break;
                }
            }

            return tax;
        }
    }
}