using Moq;

using NUnit.Framework;
using PaySpace.Calculator.Data.Models;
using PaySpace.Calculator.Services.Abstractions;

using PaySpace.Calculator.Services.Calculators;

namespace PaySpace.Calculator.Tests
{
    [TestFixture]
    internal sealed class FlatValueCalculatorTests
    {
        private Mock<ICalculatorSettingsService> _calculatorSettingsServiceMock;
        private FlatValueCalculator _calculator;
        private List<CalculatorSetting> _calculatorSettings = new List<CalculatorSetting>()
        {
        new() { Id = 7, Calculator = CalculatorType.FlatValue, RateType = RateType.Percentage, Rate = 5, From = 0, To = 199999 },
        new() { Id = 8, Calculator = CalculatorType.FlatValue, RateType = RateType.Amount, Rate = 10000, From = 200000, To = null },
        };

        [SetUp]
        public void Setup()
        {
            // Create a mock for ICalculatorSettingsService
            _calculatorSettingsServiceMock = new Mock<ICalculatorSettingsService>();
            // Mock the GetSettingsAsync method to return a list of CalculatorSetting
            _calculatorSettingsServiceMock.Setup(service => service.GetSettingsAsync(CalculatorType.FlatValue))
                                          .Returns(Task.FromResult(this._calculatorSettings));

            // Initialize FlatValueCalculator with the mock
            _calculator = new FlatValueCalculator(_calculatorSettingsServiceMock.Object);
        }

        [TestCase(199999, 9999.95)]
        [TestCase(100, 5)]
        [TestCase(200000, 10000)]
        [TestCase(6000000, 10000)]
        public async Task Calculate_Should_Return_Expected_Tax(decimal income, decimal expectedTax)
        {
            // Arrange

            // Act
            var calculateResult = await _calculator.CalculateAsync(income);

            // Assert
            Assert.AreEqual(expectedTax, calculateResult.Tax);
        }
    }
}