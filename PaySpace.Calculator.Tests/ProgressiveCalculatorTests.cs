﻿using Moq;

using NUnit.Framework;

using PaySpace.Calculator.Common.Models;
using PaySpace.Calculator.Data.Models;
using PaySpace.Calculator.Services.Abstractions;

using PaySpace.Calculator.Services.Calculators;

namespace PaySpace.Calculator.Tests
{
    [TestFixture]
    internal sealed class ProgressiveCalculatorTests
    {
        private Mock<ICalculatorSettingsService> _calculatorSettingsServiceMock;
        private ProgressiveCalculator _calculator;
        private List<CalculatorSetting> _calculatorSettings = new List<CalculatorSetting>()
        {
            new() { Id = 1, Calculator = CalculatorType.Progressive, RateType = RateType.Percentage, Rate = 10, From = 0, To = 8350 },
            new() { Id = 2, Calculator = CalculatorType.Progressive, RateType = RateType.Percentage, Rate = 15, From = 8351, To = 33950 },
            new() { Id = 3, Calculator = CalculatorType.Progressive, RateType = RateType.Percentage, Rate = 25, From = 33951, To = 82250 },
            new() { Id = 4, Calculator = CalculatorType.Progressive, RateType = RateType.Percentage, Rate = 28, From = 82251, To = 171550 },
            new() { Id = 5, Calculator = CalculatorType.Progressive, RateType = RateType.Percentage, Rate = 33, From = 171551, To = 372950 },
            new() { Id = 6, Calculator = CalculatorType.Progressive, RateType = RateType.Percentage, Rate = 35, From = 372951, To = null },
        };
        [SetUp]
        public void Setup()
        {
            // Create a mock for ICalculatorSettingsService
            _calculatorSettingsServiceMock = new Mock<ICalculatorSettingsService>();
            // Mock the GetSettingsAsync method to return a list of CalculatorSetting
            _calculatorSettingsServiceMock.Setup(service => service.GetSettingsAsync(CalculatorType.Progressive))
                                          .Returns(Task.FromResult(this._calculatorSettings));

            // Initialize FlatRateCalculator with the mock
            _calculator = new ProgressiveCalculator(_calculatorSettingsServiceMock.Object);
        }

        [TestCase(-1, 0)]
        [TestCase(50, 5)]
        [TestCase(8350.1, 835.01)] 
        [TestCase(8351, 835)]
        [TestCase(33951, 4674.85)]
        [TestCase(82251, 16749.60)]
        [TestCase(171550, 41753.32)]
        [TestCase(999999, 327681.79)]
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