using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

using Moq;

using NUnit.Framework;

using PaySpace.Calculator.Common.Models;
using PaySpace.Calculator.Data;
using PaySpace.Calculator.Data.Models;
using PaySpace.Calculator.Services;
using PaySpace.Calculator.Services.Abstractions;

namespace PaySpace.Calculator.Tests
{
    [TestFixture]
    internal sealed class CalculatorSettingsServiceTests
    {
        private ICalculatorSettingsService _calculatorSettingsService;
        private Mock<CalculatorContext> _dbContextMock = new Mock<CalculatorContext>(new DbContextOptionsBuilder<CalculatorContext>().Options);
        private IMemoryCache _memoryCache = TestUtils.GetFakeMemoryCache();

        [SetUp]
        public void Setup()
        {
            _calculatorSettingsService = new CalculatorSettingsService(this._dbContextMock.Object, _memoryCache);
        }

        [TestCase(CalculatorType.Progressive)]
        [TestCase(CalculatorType.FlatValue)]
        [TestCase(CalculatorType.FlatRate)]
        public async Task GetSettingsAsync_Should_Return_Settings_For_CalculatorType(CalculatorType calculatorType)
        {
            // Arrange

            // Act
            var calculatorSettingsResult = await _calculatorSettingsService.GetSettingsAsync(calculatorType);

            // Assert
            Assert.IsNotNull(calculatorSettingsResult);
            Assert.IsTrue(calculatorSettingsResult.Any());
        }
    }
}
