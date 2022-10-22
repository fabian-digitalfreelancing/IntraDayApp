using IntraDayApp.Domain.Enums;
using IntraDayApp.Domain.Models;

namespace IntraDayApp.Domain.Responses
{
    public class PowerServiceGetTradesResponse : ServiceResponse<IEnumerable<Trade>>
    {
        public static PowerServiceGetTradesResponse ErrorResponse(Exception error)
        {
            return new PowerServiceGetTradesResponse()
            {
                Error = error,
                Status = ServiceResponseStatus.Error,
                Data = new List<Trade>()
            };
        }

        public static PowerServiceGetTradesResponse SuccessResponse(IEnumerable<Trade> data)
        {
            return new PowerServiceGetTradesResponse()
            {
                Data = data,
                Status = ServiceResponseStatus.Success
            };
        }
    }
}
