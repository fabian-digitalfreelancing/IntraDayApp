using IntraDayApp.Domain.Enums;
using IntraDayApp.Domain.Models;

namespace IntraDayApp.Domain.Responses
{
    public class AggregateTradesResponse : ServiceResponse<IEnumerable<AggregatedTradeItem>>
    {
        public static AggregateTradesResponse ErrorResponse(Exception Error)
        {
            return new AggregateTradesResponse()
            {
                Error = Error,
                Status = ServiceResponseStatus.Error,
                Data = new List<AggregatedTradeItem>()
            };
        }

        public static AggregateTradesResponse SuccessResponse(IEnumerable<AggregatedTradeItem> Data)
        {
            return new AggregateTradesResponse()
            {
                Data = Data,
                Status = ServiceResponseStatus.Success
            };
        }
    }
}
