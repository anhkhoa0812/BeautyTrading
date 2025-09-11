namespace BT.Domain.Models.Accounts;

public class CreateAccountResponse
{
    public Guid AccountId { get; set; }
    public string Username { get; set; }
    public string AccessToken { get; set; }
}