using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyModel;

using Moq;

using NUnit.Framework;

using PaySpace.Calculator.Data;
using PaySpace.Calculator.Data.Models;
using PaySpace.Calculator.Services;
using PaySpace.Calculator.Services.Abstractions;
using PaySpace.Calculator.Services.Exceptions;

namespace PaySpace.Calculator.Tests
{
    [TestFixture]
    internal sealed class CalculatorFactoryTests
    {
        private IPostalCodeService _postalCodeService;
        private Mock<CalculatorContext> _dbContextMock = new Mock<CalculatorContext>(new DbContextOptionsBuilder<CalculatorContext>().Options);
        private Mock<IServiceProvider> _serviceProviderMock = new Mock<IServiceProvider>();
        private IMemoryCache _memoryCache = TestUtils.GetFakeMemoryCache();
        private ICalculatorFactory _calculatorFactory;

        [SetUp]
        public void Setup()
        {
            _serviceProviderMock.Setup(x => x.GetService(typeof(IFlatRateCalculator))).Returns(new Mock<IFlatRateCalculator>().Object);
            _serviceProviderMock.Setup(x => x.GetService(typeof(IFlatValueCalculator))).Returns(new Mock<IFlatValueCalculator>().Object);
            _serviceProviderMock.Setup(x => x.GetService(typeof(IProgressiveCalculator))).Returns(new Mock<IProgressiveCalculator>().Object);

            _postalCodeService = new PostalCodeService(_dbContextMock.Object, _memoryCache);

            _calculatorFactory = new CalculatorFactory(_serviceProviderMock.Object, _postalCodeService);
        }

        [TestCase("7441", typeof(IProgressiveCalculator))]
        [TestCase("A100", typeof(IFlatValueCalculator))]
        [TestCase("7000", typeof(IFlatRateCalculator))]
        [TestCase("1000", typeof(IProgressiveCalculator))]
        public async Task CalculatorTypeAsync_Should_Return_Expected_CalculatorType(string postalCode, Type expectedCalculatorType)
        {
            // Arrange

            // Act
            var calculatorTypeResult = await _calculatorFactory.GetCalculatorAsync(postalCode);

            // Assert
            Assert.IsNotNull(calculatorTypeResult);
            Assert.That(calculatorTypeResult, Is.InstanceOf(expectedCalculatorType));
        }

        [TestCase("1223", typeof(CalculatorException))]
        public async Task CalculatorTypeAsync_Should_Return_Exception_For_Unknown_PostlCode(string postalCode, Type expectedExceptionType)
        {
            // Arrange

            // Act

            // Assert
            Assert.ThrowsAsync(expectedExceptionType, async () => await _calculatorFactory.GetCalculatorAsync(postalCode));
        }
    }
}
