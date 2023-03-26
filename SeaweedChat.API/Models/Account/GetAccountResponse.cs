namespace SeaweedChat.API.Models;
public class GetAccountResponse : Response
{
    public AccountDto Account { get; set; } = null!;
}