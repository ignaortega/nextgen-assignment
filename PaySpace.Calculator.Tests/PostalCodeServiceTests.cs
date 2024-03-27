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
    internal sealed class PostalCodeServiceTests
    {
        private IPostalCodeService _postalCodeService;
        private Mock<CalculatorContext> _dbContextMock = new Mock<CalculatorContext>(new DbContextOptionsBuilder<CalculatorContext>().Options);
        private IMemoryCache memoryCache = TestUtils.GetFakeMemoryCache();
        

        [SetUp]
        public void Setup()
        {
            _postalCodeService = new PostalCodeService(this._dbContextMock.Object, memoryCache);
        }

        [TestCase("7441", CalculatorType.Progressive)]
        [TestCase("A100", CalculatorType.FlatValue)]
        [TestCase("7000", CalculatorType.FlatRate)]
        [TestCase("1000", CalculatorType.Progressive)]
        public async Task CalculatorTypeAsync_Should_Return_Expected_CalculatorType(string postalCode, CalculatorType calculatorTypeExpected)
        {
            // Arrange

            // Act
            var calculatorTypeResult = await _postalCodeService.CalculatorTypeAsync(postalCode);

            // Assert
            Assert.AreEqual(calculatorTypeExpected, calculatorTypeResult);
        }

        [TestCase("1223")]
        public async Task CalculatorTypeAsync_Should_Return_NULL_For_Unknown_PostalCode(string postalCode)
        {
            // Arrange

            // Act
            var calculatorTypeResult = await _postalCodeService.CalculatorTypeAsync(postalCode);

            // Assert
            Assert.IsNull(calculatorTypeResult);
        }
    }
}
