namespace BT.Domain.Models.Authentication.Login;

public record LoginResponse
{
    public Guid AccountId { get; set; }
    public string Username { get; set; }
    public string AccessToken { get; set; }
}