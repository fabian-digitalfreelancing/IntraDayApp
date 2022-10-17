using AutoFixture;
using AutoMapper;
using IntraDayApp.Domain.Exceptions;
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
        public async Task GetTradesAsync_ShouldReturnMapperPowerServiceResult()
        {
            var serviceResult = _fixture.CreateMany<PowerTrade>();
            var mappedResult = _fixture.CreateMany<Trade>();
            _powerService.Setup(x => x.GetTradesAsync(It.IsAny<DateTime>())).ReturnsAsync(serviceResult);
            _mapper.Setup(x => x.Map<IEnumerable<Trade>>(serviceResult)).Returns(mappedResult);

            var result = await _sut.GetTradesAsync(It.IsAny<DateTime>());

            Assert.AreEqual(mappedResult, result);
        }

        [TestMethod]
        [ExpectedException(typeof(PowerServiceWrapperException),
    "PowerService GetTradesAsync Exception")]
        public async Task GetTradesAsync_ShouldThrowPowerServiceException_WhenServiceThrowsException()
        {
            var serviceException = new PowerServiceException("Service Exception");
            _powerService.Setup(x => x.GetTradesAsync(It.IsAny<DateTime>())).ThrowsAsync(serviceException);

            await _sut.GetTradesAsync(It.IsAny<DateTime>());
        }
    }
}