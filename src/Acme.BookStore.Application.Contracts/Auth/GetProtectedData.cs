using System;

namespace Acme.BookStore.Auth;

public class GetProtectedData
{
    public required string Message { get; set; }
    public required string User { get; set; }
    public required string Tenant { get; set; }
    public DateTime Timestamp { get; set; }
}