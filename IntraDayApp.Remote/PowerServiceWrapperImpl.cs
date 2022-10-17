using AutoMapper;
using IntraDayApp.Domain.Exceptions;
using IntraDayApp.Domain.Models;
using Microsoft.Extensions.Logging;
using Services;

namespace IntraDayApp.Remote
{

    public class PowerServiceWrapperImpl : PowerServiceWrapper
    {
        private readonly IPowerService _service;
        private readonly ILogger<PowerServiceWrapperImpl> _logger;
        private readonly IMapper _mapper;
        public PowerServiceWrapperImpl(IPowerService service,
            ILogger<PowerServiceWrapperImpl> logger,
            IMapper mapper) => (_service, _logger, _mapper) = (service, logger, mapper);
        public async Task<IEnumerable<Trade>> GetTradesAsync(DateTime date)
        {

            try
            {
                var result = await _service.GetTradesAsync(date);
                return _mapper.Map<IEnumerable<Trade>>(result);
            }
            catch (PowerServiceException ex)
            {
                _logger.LogError("Power Service GetTradesAsync error with date {date}: {message}", ex.Message, date);
                throw new PowerServiceWrapperException("PowerService GetTradesAsync Exception", ex);
            }
        }
    }
}