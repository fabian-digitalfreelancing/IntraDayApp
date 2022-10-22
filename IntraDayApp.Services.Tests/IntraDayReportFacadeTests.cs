using AutoFixture;
using IntraDayApp.Domain.Interfaces.Remote;
using IntraDayApp.Domain.Interfaces.Service;
using IntraDayApp.Domain.Models;
using IntraDayApp.Domain.Responses;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntraDayApp.Service.Tests
{
    [TestClass]
    public class IntraDayReportFacadeTests
    {
        private readonly IntraDayReportFacadeImpl _sut;
        private readonly Mock<PowerServiceWrapper> _powerService = new();
        private readonly Mock<PowerServiceAggregator> _aggregator = new();
        private readonly Mock<ReportService> _reportService = new();
        private readonly Mock<TimeProvider> _timeProvider = new();
        private readonly Mock<ILogger<IntraDayReportFacadeImpl>> _logger = new();

        private readonly Fixture _fixture = new();

        private readonly string fileLocation = "location.csv";
        private readonly IEnumerable<Trade> _trades;
        private readonly IEnumerable<AggregatedTradeItem> _aggregatedTrades;
        private readonly PowerServiceGetTradesResponse _powerServiceSuccessResponse;
        private readonly PowerServiceGetTradesResponse _powerServiceErrorResponse;
        private readonly AggregateTradesResponse _aggregateTradesSuccessResponse;
        private readonly AggregateTradesResponse _aggregateTradesErrorResponse;
        private readonly CreateReportResponse _createReportSuccessResponse;
        public IntraDayReportFacadeTests()
        {
            _trades = _fixture.CreateMany<Trade>();
            _timeProvider.Setup(x => x.Now()).Returns(DateTime.Now);
            _timeProvider.Setup(x => x.TodaysDate()).Returns(DateTime.Today.Date);
            _powerServiceSuccessResponse = PowerServiceGetTradesResponse.SuccessResponse(_trades);
            _powerServiceErrorResponse = PowerServiceGetTradesResponse.ErrorResponse(new Exception());
            _aggregatedTrades = _fixture.CreateMany<AggregatedTradeItem>();
            _aggregateTradesSuccessResponse = AggregateTradesResponse.SuccessResponse(_aggregatedTrades);
            _aggregateTradesErrorResponse = AggregateTradesResponse.ErrorResponse(new Exception());
            _createReportSuccessResponse = CreateReportResponse.SuccessResponse(fileLocation);

            _sut = new IntraDayReportFacadeImpl(_powerService.Object, _aggregator.Object, _reportService.Object, _timeProvider.Object, _logger.Object);
        }

        [TestMethod]
        public async Task Create_ShouldCallPowerServiceWithTodaysDate()
        {

            _powerService.Setup(x => x.GetTradesAsync(DateTime.Today)).ReturnsAsync(_powerServiceSuccessResponse);
            _aggregator.Setup(x => x.Aggregate(_trades)).Returns(_aggregateTradesSuccessResponse);
            _reportService.Setup(x => x.CreateReport(_aggregatedTrades, fileLocation)).Returns(_createReportSuccessResponse);

            await _sut.CreateCsvIntraDayReportAsync(fileLocation);

            _powerService.Verify(x => x.GetTradesAsync(DateTime.Today), Times.Once());
        }

        [TestMethod]
        public async Task Create_ShouldAggregatePowerServiceResponse()
        {
            _powerService.Setup(x => x.GetTradesAsync(DateTime.Today)).ReturnsAsync(_powerServiceSuccessResponse);
            _aggregator.Setup(x => x.Aggregate(_trades)).Returns(_aggregateTradesSuccessResponse);
            _reportService.Setup(x => x.CreateReport(_aggregatedTrades, fileLocation)).Returns(_createReportSuccessResponse);

            await _sut.CreateCsvIntraDayReportAsync(fileLocation);

            _aggregator.Verify(x => x.Aggregate(_trades), Times.Once());
        }


        [TestMethod]
        public async Task Create_ShouldNotAggregatePowerServiceResponse_WhenPowerServiceReturnsError()
        {
            _powerService.Setup(x => x.GetTradesAsync(DateTime.Today)).ReturnsAsync(_powerServiceErrorResponse);

            await _sut.CreateCsvIntraDayReportAsync(fileLocation);

            _aggregator.Verify(x => x.Aggregate(It.IsAny<IEnumerable<Trade>>()), Times.Never());
        }

        [TestMethod]
        public async Task Create_ShouldCreateCsvFromAggregatedResults()
        {
            _powerService.Setup(x => x.GetTradesAsync(DateTime.Today)).ReturnsAsync(_powerServiceSuccessResponse);
            _aggregator.Setup(x => x.Aggregate(_trades)).Returns(_aggregateTradesSuccessResponse);
            _reportService.Setup(x => x.CreateReport(_aggregatedTrades, It.IsAny<string>())).Returns(_createReportSuccessResponse);

            await _sut.CreateCsvIntraDayReportAsync(fileLocation);

            _reportService.Verify(x => x.CreateReport(_aggregatedTrades, fileLocation), Times.Once());
        }

        [TestMethod]
        public async Task Create_ShouldNotCreateCsvFromAggregatedResults_WhenAggregatorReturnsError()
        {
            _powerService.Setup(x => x.GetTradesAsync(DateTime.Today)).ReturnsAsync(_powerServiceSuccessResponse);
            _aggregator.Setup(x => x.Aggregate(_trades)).Returns(_aggregateTradesErrorResponse);

            await _sut.CreateCsvIntraDayReportAsync(fileLocation);

            _reportService.Verify(x => x.CreateReport(It.IsAny<IEnumerable<AggregatedTradeItem>>(), fileLocation), Times.Never());
        }
    }
}
