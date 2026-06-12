namespace SecurityLab.Api.Common;

public abstract class AppException : Exception
{
    protected AppException(string message) : base(message) { }
}

public class NotFoundException     : AppException { public NotFoundException(string m)     : base(m) { } }
public class ConflictException     : AppException { public ConflictException(string m)     : base(m) { } }
public class UnauthorizedException : AppException { public UnauthorizedException(string m) : base(m) { } }
