using IntraDayApp.Domain.Enums;
using IntraDayApp.Domain.Models;

namespace IntraDayApp.Domain.Responses
{
    public class PowerServiceGetTradesResponse : ServiceResponse<IEnumerable<Trade>>
    {
        public static PowerServiceGetTradesResponse ErrorResponse(Exception Error)
        {
            return new PowerServiceGetTradesResponse()
            {
                Error = Error,
                Status = ServiceResponseStatus.Error
            };
        }

        public static PowerServiceGetTradesResponse SuccessResponse(IEnumerable<Trade> Data)
        {
            return new PowerServiceGetTradesResponse()
            {
                Data = Data,
                Status = ServiceResponseStatus.Success
            };
        }
    }
}
