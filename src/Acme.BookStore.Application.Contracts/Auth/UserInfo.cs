namespace Acme.BookStore.Auth;

public class UserInfo
{
    public required string Username { get; set; }
    public required string Tenant { get; set; }
    public string[] Claims { get; set; } = [];
}