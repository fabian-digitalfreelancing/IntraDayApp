using IntraDayApp.Domain.Enums;

namespace IntraDayApp.Domain.Responses
{
    public class ServiceResponse<T>
    {
        public T? Data { get; set; }
        public ServiceResponseStatus Status { get; set; }
        public Exception? Error { get; set; }
    }
}
