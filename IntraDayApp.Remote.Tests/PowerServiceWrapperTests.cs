using AutoFixture;
using AutoMapper;
using IntraDayApp.Domain.Enums;
using IntraDayApp.Domain.Models;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntraDayApp.Remote.Tests
{
    [TestClass]
    public class PowerServiceWrapperTests
    {
        private readonly PowerServiceWrapperImpl _sut;
        private readonly Mock<IPowerService> _powerService = new Mock<IPowerService>();
        private readonly Mock<ILogger<PowerServiceWrapperImpl>> _logger = new Mock<ILogger<PowerServiceWrapperImpl>>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly Fixture _fixture = new Fixture();

        public PowerServiceWrapperTests()
        {
            _sut = new PowerServiceWrapperImpl(_powerService.Object, _logger.Object, _mapper.Object);
        }

        [TestMethod]
        public async Task GetTradesAsync_ShouldCallPowerServiceWithDate()
        {
            var date = _fixture.Create<DateTime>();

            await _sut.GetTradesAsync(date);

            _powerService.Verify(x => x.GetTradesAsync(date), Times.Once());
        }

        [TestMethod]
        public async Task GetTradesAsync_ShouldReturnsSuccessResponseWithMappedPowerServiceResult()
        {

            var serviceResult = _fixture.CreateMany<PowerTrade>();
            var mappedResult = _fixture.CreateMany<Trade>();
            _mapper.Setup(x => x.Map<IEnumerable<Trade>>(serviceResult)).Returns(mappedResult);
            _powerService.Setup(x => x.GetTradesAsync(It.IsAny<DateTime>())).ReturnsAsync(serviceResult);

            var result = await _sut.GetTradesAsync(It.IsAny<DateTime>());

            Assert.AreEqual(mappedResult, result.Data);
            Assert.AreEqual(ServiceResponseStatus.Success, result.Status);
        }

        [TestMethod]
        public async Task GetTradesAsync_ShouldReturnErrorResponse_WhenPowerServiceThrowsException()
        {
            var serviceException = new PowerServiceException("Service Exception");
            _powerService.Setup(x => x.GetTradesAsync(It.IsAny<DateTime>())).ThrowsAsync(serviceException);

            var result = await _sut.GetTradesAsync(It.IsAny<DateTime>());

            Assert.AreEqual(serviceException, result.Error);
            Assert.AreEqual(ServiceResponseStatus.Error, result.Status);
        }

        [TestMethod]
        public async Task GetTradesAsync_ShouldLogError_WhenPowerServiceThrowsException()
        {
            var serviceException = new PowerServiceException("Service Exception");
            var date = _fixture.Create<DateTime>();
            _powerService.Setup(x => x.GetTradesAsync(date)).ThrowsAsync(serviceException);

            var result = await _sut.GetTradesAsync(date);

            VerifyLoggerErrorCalledWithMessage($"Power Service GetTradesAsync error input:{date.ToUniversalTime()} message:{serviceException.Message}", Times.Once());
        }

        [TestMethod]
        public async Task GetTradesAsync_ShouldLogError_WhenMapperThrowsException()
        {
            var mapperException = new AutoMapperMappingException("Mapping Exception");
            var serviceResult = _fixture.CreateMany<PowerTrade>();
            _powerService.Setup(x => x.GetTradesAsync(It.IsAny<DateTime>())).ReturnsAsync(serviceResult);
            _mapper.Setup(x => x.Map<IEnumerable<Trade>>(serviceResult)).Throws(mapperException);

            var result = await _sut.GetTradesAsync(It.IsAny<DateTime>());

            VerifyLoggerErrorCalledWithMessage($"GetTradesAsync Mapping message:{mapperException.Message}", Times.Once());
        }

        [TestMethod]
        public async Task GetTradesAsync_ShouldReturnErrorResponse_WhenMapperThrowsException()
        {
            var mapperException = new AutoMapperMappingException("Mapping Exception");
            var serviceResult = _fixture.CreateMany<PowerTrade>();
            _powerService.Setup(x => x.GetTradesAsync(It.IsAny<DateTime>())).ReturnsAsync(serviceResult);
            _mapper.Setup(x => x.Map<IEnumerable<Trade>>(serviceResult)).Throws(mapperException);

            var result = await _sut.GetTradesAsync(It.IsAny<DateTime>());

            Assert.AreEqual(mapperException, result.Error);
            Assert.AreEqual(ServiceResponseStatus.Error, result.Status);
        }

        [TestMethod]
        public async Task GetTradesAsync_ShouldLogError_WhenGenericExceptionThrown()
        {
            var exception = new Exception("Exception");
            var serviceResult = _fixture.CreateMany<PowerTrade>();
            var date = _fixture.Create<DateTime>();
            _powerService.Setup(x => x.GetTradesAsync(date)).ReturnsAsync(serviceResult);
            _mapper.Setup(x => x.Map<IEnumerable<Trade>>(serviceResult)).Throws(exception);

            var result = await _sut.GetTradesAsync(date);

            VerifyLoggerErrorCalledWithMessage($"GetTradesAsync error input: {date.ToUniversalTime()} message: {exception.Message}", Times.Once());
        }

        [TestMethod]
        public async Task GetTradesAsync_ShouldReturnErrorResponse_WhenGenericExceptionThrown()
        {
            var exception = new Exception("Mapping Exception");
            var serviceResult = _fixture.CreateMany<PowerTrade>();
            _powerService.Setup(x => x.GetTradesAsync(It.IsAny<DateTime>())).ReturnsAsync(serviceResult);
            _mapper.Setup(x => x.Map<IEnumerable<Trade>>(serviceResult)).Throws(exception);

            var result = await _sut.GetTradesAsync(It.IsAny<DateTime>());

            Assert.AreEqual(exception, result.Error);
            Assert.AreEqual(ServiceResponseStatus.Error, result.Status);
        }

        private void VerifyLoggerErrorCalledWithMessage(string message, Moq.Times times)
        {
            _logger.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
                It.Is<EventId>(eventId => eventId.Id == 0),
                It.Is<It.IsAnyType>((@object, @type) =>
                    @object.ToString() == message &&
                    @type.Name == "FormattedLogValues"),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once());
        }
    }
}