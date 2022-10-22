using AutoFixture;
using IntraDayApp.Domain.Models;
using IntraDayApp.Domain.Responses;
using IntraDayApp.Remote;
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
        private readonly Mock<PowerServiceWrapper> _powerService = new Mock<PowerServiceWrapper>();
        private readonly Mock<PowerServiceAggregator> _aggregator = new Mock<PowerServiceAggregator>();
        private readonly Mock<CsvCreator> _csvCreator = new Mock<CsvCreator>();
        private readonly Mock<ILogger<IntraDayReportFacadeImpl>> _logger = new Mock<ILogger<IntraDayReportFacadeImpl>>();

        private readonly Fixture _fixture = new Fixture();

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
            _powerServiceSuccessResponse = PowerServiceGetTradesResponse.SuccessResponse(_trades);
            _powerServiceErrorResponse = PowerServiceGetTradesResponse.ErrorResponse(new Exception());
            _aggregatedTrades = _fixture.CreateMany<AggregatedTradeItem>();
            _aggregateTradesSuccessResponse = AggregateTradesResponse.SuccessResponse(_aggregatedTrades);
            _aggregateTradesErrorResponse = AggregateTradesResponse.ErrorResponse(new Exception());
            _createReportSuccessResponse = CreateReportResponse.SuccessResponse(fileLocation);

            _sut = new IntraDayReportFacadeImpl(_powerService.Object, _aggregator.Object, _csvCreator.Object, _logger.Object);
        }

        [TestMethod]
        public async Task Create_ShouldCallPowerServiceWithTodaysDate()
        {
            _powerService.Setup(x => x.GetTradesAsync(DateTime.Today)).ReturnsAsync(_powerServiceSuccessResponse);
            _aggregator.Setup(x => x.Aggregate(_trades)).Returns(_aggregateTradesSuccessResponse);
            _csvCreator.Setup(x => x.CreateReport(_aggregatedTrades, fileLocation)).Returns(_createReportSuccessResponse);

            await _sut.CreateCsvIntraDayReportAsync(fileLocation);

            _powerService.Verify(x => x.GetTradesAsync(DateTime.Today), Times.Once());
        }

        [TestMethod]
        public async Task Create_ShouldAggregatePowerServiceResponse()
        {
            _powerService.Setup(x => x.GetTradesAsync(DateTime.Today)).ReturnsAsync(_powerServiceSuccessResponse);
            _aggregator.Setup(x => x.Aggregate(_trades)).Returns(_aggregateTradesSuccessResponse);
            _csvCreator.Setup(x => x.CreateReport(_aggregatedTrades, fileLocation)).Returns(_createReportSuccessResponse);

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
            _csvCreator.Setup(x => x.CreateReport(_aggregatedTrades, It.IsAny<string>())).Returns(_createReportSuccessResponse);

            await _sut.CreateCsvIntraDayReportAsync(fileLocation);

            _csvCreator.Verify(x => x.CreateReport(_aggregatedTrades, fileLocation), Times.Once());
        }

        [TestMethod]
        public async Task Create_ShouldNotCreateCsvFromAggregatedResults_WhenAggregatorReturnsError()
        {
            _powerService.Setup(x => x.GetTradesAsync(DateTime.Today)).ReturnsAsync(_powerServiceSuccessResponse);
            _aggregator.Setup(x => x.Aggregate(_trades)).Returns(_aggregateTradesErrorResponse);

            await _sut.CreateCsvIntraDayReportAsync(fileLocation);

            _csvCreator.Verify(x => x.CreateReport(It.IsAny<IEnumerable<AggregatedTradeItem>>(), fileLocation), Times.Never());
        }
    }
}
