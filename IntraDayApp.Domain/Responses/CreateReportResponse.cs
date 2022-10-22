using IntraDayApp.Domain.Enums;

namespace IntraDayApp.Domain.Responses
{
    public class CreateReportResponse : ServiceResponse<string>
    {
        public static CreateReportResponse ErrorResponse(Exception Error)
        {
            return new CreateReportResponse()
            {
                Error = Error,
                Status = ServiceResponseStatus.Error,
                Data = String.Empty
            };
        }

        public static CreateReportResponse SuccessResponse(string Data)
        {
            return new CreateReportResponse()
            {
                Data = Data,
                Status = ServiceResponseStatus.Success
            };
        }
    }
}
