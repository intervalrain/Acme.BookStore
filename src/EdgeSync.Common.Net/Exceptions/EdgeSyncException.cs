namespace EdgeSync.Common.Net.Exceptions;

/// <summary>
/// Base class for all EdgeSync exceptions
/// </summary>
public abstract class EdgeSyncException : Exception
{
    protected EdgeSyncException(string message) : base(message) { }
}

/// <summary>
/// Exception for authorization errors
/// Status code: 401
/// </summary>
public class EdgeSyncAuthorizationException : EdgeSyncException
{
    public EdgeSyncAuthorizationException(string message) : base(message) { }
}

/// <summary>
/// Exception for bad request errors
/// Status code: 400
/// </summary>
public class EdgeSyncBadRequestException : EdgeSyncException
{
    public EdgeSyncBadRequestException(string message) : base(message) { }
}

/// <summary>
/// Exception for forbidden errors
/// Status code: 403
/// </summary>
public class EdgeSyncForbiddenException : EdgeSyncException
{
    public EdgeSyncForbiddenException(string message) : base(message) { }
}

/// <summary>
/// Exception for not found errors
/// Status code: 404
/// </summary>
public class EdgeSyncNotFoundException : EdgeSyncException
{
    public EdgeSyncNotFoundException(string message) : base(message) { }
}

/// <summary>
/// Exception for conflict errors
/// Status code: 409
/// </summary>
public class EdgeSyncConflictException : EdgeSyncException
{
    public EdgeSyncConflictException(string message) : base(message) { }
}

/// <summary>
/// Exception for unprocessable entity errors
/// Status code: 422
/// </summary>
public class EdgeSyncUnprocessableEntityException : EdgeSyncException
{
    public EdgeSyncUnprocessableEntityException(string message) : base(message) { }
}