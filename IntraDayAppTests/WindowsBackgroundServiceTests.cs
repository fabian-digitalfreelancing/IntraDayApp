using IntraDayApp;
using IntraDayApp.Domain.AppSettings;
using IntraDayApp.Domain.Interfaces.App;
using IntraDayApp.Domain.Interfaces.Service;
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

        private readonly Mock<IntraDayReportFacade> _reportFacade = new();
        private readonly Mock<ILogger<WindowsBackgroundService>> _logger = new();
        private readonly Mock<TimerWrapper> _timer = new();
        private readonly ReportSettings _options = new()
        {
            Location = "location",
            Frequency = 1
        };

        public WindowsBackgroundServiceTests()
        {
            _sut = new WindowsBackgroundService(_reportFacade.Object, _logger.Object, Options.Create(_options), _timer.Object);
        }
        [TestMethod]
        public void ShouldCreateReportAtLocationFromOptions()
        {
            _sut = new WindowsBackgroundService(_reportFacade.Object, _logger.Object, Options.Create(_options), _timer.Object);
            var tokenSource = new CancellationTokenSource();
            tokenSource.CancelAfter(100);

            _sut.StartAsync(tokenSource.Token);

            _reportFacade.Verify(x => x.CreateCsvIntraDayReportAsync(_options.Location), Times.AtLeastOnce());
        }

        [TestMethod]
        public void ShouldCallCreateReportOnceIfCancellationRequested()
        {
            var tokenSource = new CancellationTokenSource();
            tokenSource.Cancel();

            _sut.StartAsync(tokenSource.Token);

            _reportFacade.Verify(x => x.CreateCsvIntraDayReportAsync(_options.Location), Times.Once());
        }
    }
}