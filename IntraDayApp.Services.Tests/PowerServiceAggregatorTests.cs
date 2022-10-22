using IntraDayApp.Domain.Enums;
using IntraDayApp.Service;
using IntraDayApp.Service.Tests;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;

namespace IntraDayApp.Services.Tests
{
    [TestClass]
    public class PowerServiceAggregatorTests
    {
        private readonly PowerServiceAggregatorImpl _sut;
        private readonly Mock<ILogger<PowerServiceAggregatorImpl>> _logger = new();

        public PowerServiceAggregatorTests()
        {
            _sut = new PowerServiceAggregatorImpl(_logger.Object);
        }
        [TestMethod]
        public void Aggregate_ShouldSumVolumesForMatchingPeriods()
        {
            var input = AggregatorTestData.TwoTradesWithTwoMatchingPeriods;
            var expected = AggregatorTestData.TwoTradesWithTwoMatchingPeriodsExpectedResult;

            var result = _sut.Aggregate(input);

            Assert.AreEqual(ServiceResponseStatus.Success, result.Status);
            CollectionAssert.AreEqual(expected.ToList(), result.Data.ToList());
        }

        [TestMethod]
        public void Aggregate_ShouldSumVolumesForNotMatchingPeriods()
        {
            var input = AggregatorTestData.TwoTradesWithTwoNonMatchingPeriods;
            var expected = AggregatorTestData.TwoTradesWithTwoNonMatchingPeriodsExpectedResult;

            var result = _sut.Aggregate(input);

            CollectionAssert.AreEqual(expected.ToList(), result.Data.ToList());
        }

        [TestMethod]
        public void Aggregate_ShouldReturnErrorResponseAndLogErrorOnNegativePeriods()
        {
            var input = AggregatorTestData.TradeWithNegativePeriods;
            var result = _sut.Aggregate(input);

            Assert.AreEqual(ServiceResponseStatus.Error, result.Status);
            VerifyLoggerErrorCalledWithMessage($"GetTimeFromPeriod out of range error with period: {input.ToList()[0].Periods[0].Period}");
        }

        [TestMethod]
        public void Aggregate_ShouldReturnErrorResponseAndLogErrorOnTooHighPeriods()
        {
            var input = AggregatorTestData.TradeWithTooHighPeriods;

            var result = _sut.Aggregate(input);

            Assert.AreEqual(ServiceResponseStatus.Error, result.Status);
            VerifyLoggerErrorCalledWithMessage($"GetTimeFromPeriod out of range error with period: {input.ToList()[0].Periods[0].Period}");
        }

        [TestMethod]
        public void Aggregate_ShouldReturnErrorResponseAndLogErrorOnZeroValuePeriods()
        {
            var input = AggregatorTestData.TradeWithZeroValuePeriods;

            var result = _sut.Aggregate(input);

            Assert.AreEqual(ServiceResponseStatus.Error, result.Status);
            VerifyLoggerErrorCalledWithMessage($"GetTimeFromPeriod out of range error with period: {input.ToList()[0].Periods[0].Period}");
        }

        private void VerifyLoggerErrorCalledWithMessage(string message)
        {
            _logger.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
                It.Is<EventId>(eventId => eventId.Id == 0),
                It.Is<It.IsAnyType>((@object, type) =>
                    @object.ToString() == message &&
                    type.Name == "FormattedLogValues"),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once());
        }
    }
}