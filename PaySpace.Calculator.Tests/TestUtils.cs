using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Caching.Memory;

using PaySpace.Calculator.Common.Models;
using PaySpace.Calculator.Data.Models;

namespace PaySpace.Calculator.Tests
{
    internal static class TestUtils
    {
        private static List<CalculatorSetting> _calculatorSettingsProgressive = new List<CalculatorSetting>()
        {
            new() { Id = 1, Calculator = CalculatorType.Progressive, RateType = RateType.Percentage, Rate = 10, From = 0, To = 8350 },
            new() { Id = 2, Calculator = CalculatorType.Progressive, RateType = RateType.Percentage, Rate = 15, From = 8351, To = 33950 },
            new() { Id = 3, Calculator = CalculatorType.Progressive, RateType = RateType.Percentage, Rate = 25, From = 33951, To = 82250 },
            new() { Id = 4, Calculator = CalculatorType.Progressive, RateType = RateType.Percentage, Rate = 28, From = 82251, To = 171550 },
            new() { Id = 5, Calculator = CalculatorType.Progressive, RateType = RateType.Percentage, Rate = 33, From = 171551, To = 372950 },
            new() { Id = 6, Calculator = CalculatorType.Progressive, RateType = RateType.Percentage, Rate = 35, From = 372951, To = null }
        };

        private static List<CalculatorSetting> _calculatorSettingsFlatValue = new List<CalculatorSetting>()
        {
            new() { Id = 7, Calculator = CalculatorType.FlatValue, RateType = RateType.Percentage, Rate = 5, From = 0, To = 200000 },
            new() { Id = 8, Calculator = CalculatorType.FlatValue, RateType = RateType.Amount, Rate = 10000, From = 200000, To = null }
        };

        private static List<CalculatorSetting> _calculatorSettingsFlatRate = new List<CalculatorSetting>()
        {
            new() { Id = 9, Calculator = CalculatorType.FlatRate, RateType = RateType.Percentage, Rate = 17.5M, From = 0, To = null }
        };

        private static IList<PostalCode> _postalCodes = new List<PostalCode>()
        {
            new() { Id = 1, Calculator = CalculatorType.Progressive, Code = "7441" },
            new() { Id = 2, Calculator = CalculatorType.FlatValue, Code = "A100" },
            new() { Id = 3, Calculator = CalculatorType.FlatRate, Code = "7000" },
            new() { Id = 4, Calculator = CalculatorType.Progressive, Code = "1000" },
        };

        public static IMemoryCache GetFakeMemoryCache()
        {
            IMemoryCache memoryCache = new FakeMemoryCache();
            memoryCache.Set($"CalculatorSetting:{CalculatorType.Progressive}", _calculatorSettingsProgressive);
            memoryCache.Set($"CalculatorSetting:{CalculatorType.FlatValue}", _calculatorSettingsFlatValue);
            memoryCache.Set($"CalculatorSetting:{CalculatorType.FlatRate}", _calculatorSettingsFlatRate);
            memoryCache.Set("PostalCodes", _postalCodes);

            return memoryCache;
        }
    }
}
