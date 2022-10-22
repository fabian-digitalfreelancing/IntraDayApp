using IntraDayApp.Domain.Enums;

namespace IntraDayApp.Domain.Responses
{
    public class CreateReportResponse : ServiceResponse<string>
    {
        public static CreateReportResponse ErrorResponse(Exception error)
        {
            return new CreateReportResponse()
            {
                Error = error,
                Status = ServiceResponseStatus.Error,
                Data = String.Empty
            };
        }

        public static CreateReportResponse SuccessResponse(string data)
        {
            return new CreateReportResponse()
            {
                Data = data,
                Status = ServiceResponseStatus.Success
            };
        }
    }
}
