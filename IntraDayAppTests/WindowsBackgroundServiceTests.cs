using IntraDayApp;
using IntraDayApp.Domain.AppSettings;
using IntraDayApp.Service;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading;

namespace IntraDayAppTests
{
    [TestClass]
    public class WindowsBackgroundServiceTests
    {
        private WindowsBackgroundService _sut;

        private readonly Mock<IntraDayReportFacade> _reportFacade = new Mock<IntraDayReportFacade>();
        private readonly Mock<ILogger<WindowsBackgroundService>> _logger = new Mock<ILogger<WindowsBackgroundService>>();
        private readonly Mock<WorkerHelper> _helper = new Mock<WorkerHelper>();
        private readonly ReportSettings _options = new ReportSettings
        {
            Location = "location",
            Frequency = 1
        };

        public WindowsBackgroundServiceTests()
        {
            _sut = new WindowsBackgroundService(_reportFacade.Object, _logger.Object, Options.Create(_options), _helper.Object);
        }
        [TestMethod]
        public void ShouldCreateReportAtLocationFromOptions()
        {
            _sut = new WindowsBackgroundService(_reportFacade.Object, _logger.Object, Options.Create(_options), _helper.Object);
            var tokenSource = new CancellationTokenSource();
            tokenSource.CancelAfter(100);

            var task = _sut.StartAsync(tokenSource.Token);

            _reportFacade.Verify(x => x.CreateCsvIntraDayReportAsync(_options.Location), Times.AtLeastOnce());
        }

        [TestMethod]
        public void ShouldNotCreateReportIfCancellationRequested()
        {
            var tokenSource = new CancellationTokenSource();
            tokenSource.Cancel();

            var task = _sut.StartAsync(tokenSource.Token);

            _reportFacade.Verify(x => x.CreateCsvIntraDayReportAsync(_options.Location), Times.Never());
        }
    }
}