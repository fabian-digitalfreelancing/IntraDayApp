using AutoFixture;
using IntraDayApp.Domain.Enums;
using IntraDayApp.Domain.Interfaces.Service;
using IntraDayApp.Domain.Models;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace IntraDayApp.Service.Tests
{
    [TestClass]
    public class ReportServiceTests
    {
        private readonly ReportServiceImpl _sut;
        private readonly Fixture _fixture = new();
        private readonly Mock<ILogger<ReportServiceImpl>> _logger = new();
        private readonly Mock<CsvServiceWrapper> _csvService = new();
        private readonly Mock<TimeProvider> _timeProvider = new();

        private readonly IEnumerable<AggregatedTradeItem> _tradeItems;
        private readonly string _fileLocation;
        private readonly string _expectedName;

        public ReportServiceTests()
        {
            _fileLocation = "somelocation/folder";
            var date = new DateTime(2022, 10, 10, 1, 1, 0);
            var fileName = "PowerPosition_20221010_0101.csv";
            _expectedName = _fileLocation + "/" + fileName;

            _tradeItems = _fixture.CreateMany<AggregatedTradeItem>();
            _timeProvider.Setup(x => x.Now()).Returns(date);

            _sut = new ReportServiceImpl(_logger.Object, _csvService.Object, _timeProvider.Object);
        }

        [TestMethod]
        public void CreateReport_ShouldCallCsvServiceWithTradeItemsAndCorrectFilepath()
        {
            _sut.CreateReport(_tradeItems, _fileLocation);

            _csvService.Verify(x => x.WriteToFile(_tradeItems, _expectedName), Times.Once());
        }

        [TestMethod]
        public void CreateReport_ShouldReturnSuccessWithFilePath()
        {
            var result = _sut.CreateReport(_tradeItems, _fileLocation);

            Assert.AreEqual(ServiceResponseStatus.Success, result.Status);
            Assert.AreEqual(_expectedName, result.Data);
        }

        [TestMethod]
        public void CreateReport_ShouldReturnError_WhenCsvServiceReturnException()
        {
            var exception = new Exception();
            _csvService.Setup(x => x.WriteToFile(_tradeItems, It.IsAny<string>())).Throws(exception);
            var result = _sut.CreateReport(_tradeItems, _fileLocation);

            Assert.AreEqual(ServiceResponseStatus.Error, result.Status);
            Assert.AreEqual(exception, result.Error);
        }
    }
}
