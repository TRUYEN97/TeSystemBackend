namespace TeSystemBackend.Service.Exceptions
{
    public sealed class BadRequestException : AppException
    {
        public BadRequestException(string message)
            : base(400, message)
        {
        }
    }
}

