using System.Security.Claims;
namespace SeaweedChat.API.Security;
public interface IAuthentication
{
    void Configure(IServiceCollection services);
    string Authenticate(IEnumerable<Claim> claims);
}