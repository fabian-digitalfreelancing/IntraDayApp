using AutoMapper;
using IntraDayApp.Domain.Models;
using IntraDayApp.Domain.Responses;
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
        public async Task<PowerServiceGetTradesResponse> GetTradesAsync(DateTime date)
        {
            try
            {
                return await GetMappedTradesResponse(date);
            }
            catch (PowerServiceException ex)
            {
                return LogAndGetErrorResponse($"Power Service GetTradesAsync error input:{date.ToUniversalTime()} message:{ex.Message}", ex);
            }
            catch (AutoMapperMappingException ex)
            {
                return LogAndGetErrorResponse($"GetTradesAsync Mapping message:{ex.Message}", ex);
            }
            catch (Exception ex)
            {
                return LogAndGetErrorResponse($"GetTradesAsync error input: {date.ToUniversalTime()} message: {ex.Message}", ex);
            }
        }

        private async Task<PowerServiceGetTradesResponse> GetMappedTradesResponse(DateTime date)
        {
            var result = await _service.GetTradesAsync(date);
            var mappedResult = _mapper.Map<IEnumerable<Trade>>(result);
            return PowerServiceGetTradesResponse.SuccessResponse(mappedResult);
        }
        private PowerServiceGetTradesResponse LogAndGetErrorResponse(string message, Exception ex)
        {
            _logger.LogError(message);
            return PowerServiceGetTradesResponse.ErrorResponse(ex);
        }
    }
}