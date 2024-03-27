using Moq;

using NUnit.Framework;

using PaySpace.Calculator.Common.Models;
using PaySpace.Calculator.Data.Models;
using PaySpace.Calculator.Services.Abstractions;
using PaySpace.Calculator.Services.Calculators;

namespace PaySpace.Calculator.Tests
{
    [TestFixture]
    internal sealed class FlatRateCalculatorTests
    {
        private Mock<ICalculatorSettingsService> _calculatorSettingsServiceMock;
        private FlatRateCalculator _calculator;
        private List<CalculatorSetting> _calculatorSettings = new List<CalculatorSetting>()
        {
            new() { Id = 9, Calculator = CalculatorType.FlatRate, RateType = RateType.Percentage, Rate = 17.5M, From = 0, To = null }
        };


        [SetUp]
        public void Setup()
        {
            // Create a mock for ICalculatorSettingsService
            _calculatorSettingsServiceMock = new Mock<ICalculatorSettingsService>();
            // Mock the GetSettingsAsync method to return a list of CalculatorSetting
            _calculatorSettingsServiceMock.Setup(service => service.GetSettingsAsync(CalculatorType.FlatRate))
                                          .Returns(Task.FromResult(this._calculatorSettings));

            // Initialize FlatRateCalculator with the mock
            _calculator = new FlatRateCalculator(_calculatorSettingsServiceMock.Object);
        }

        [TestCase(999999, 174999.825)]
        [TestCase(1000, 175)]
        [TestCase(5, 0.875)]
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
