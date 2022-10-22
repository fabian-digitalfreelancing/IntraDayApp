using IntraDayApp.Domain.Enums;
using IntraDayApp.Domain.Models;

namespace IntraDayApp.Domain.Responses
{
    public class AggregateTradesResponse : ServiceResponse<IEnumerable<AggregatedTradeItem>>
    {
        public static AggregateTradesResponse ErrorResponse(Exception error)
        {
            return new AggregateTradesResponse()
            {
                Error = error,
                Status = ServiceResponseStatus.Error,
                Data = new List<AggregatedTradeItem>()
            };
        }

        public static AggregateTradesResponse SuccessResponse(IEnumerable<AggregatedTradeItem> data)
        {
            return new AggregateTradesResponse()
            {
                Data = data,
                Status = ServiceResponseStatus.Success
            };
        }
    }
}
