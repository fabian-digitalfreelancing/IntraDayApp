namespace IntraDayApp.Domain.Exceptions
{
    public class PowerServiceWrapperException : Exception
    {
        public PowerServiceWrapperException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
