namespace TeSystemBackend.Service.Exceptions
{
    public sealed class UnauthorizedException : AppException
    {
        public UnauthorizedException(string message)
            : base(401, message)
        {
        }
    }
}

