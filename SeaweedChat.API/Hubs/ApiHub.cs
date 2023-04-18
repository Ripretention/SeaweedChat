using Microsoft.AspNetCore.SignalR;
using SeaweedChat.Domain.Aggregates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
namespace SeaweedChat.API.Hubs;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ApiHub<T> : Hub<T> where T : class
{
    protected readonly ILogger? _logger;
    protected readonly IUserRepository _usrRepository;
    public ApiHub(
        IUserRepository usrRepository,
        ILogger? logger
    )
    {
        _usrRepository = usrRepository ?? throw new ArgumentNullException(nameof(usrRepository));
        _logger = logger;
    }

    protected Guid CurrentUserId 
    {
        get
        {
            Guid userId = Guid.Empty;
            if (Context.User != null)
                foreach (var identity in Context.User.Identities)
                    Guid.TryParse(identity.Claims.FirstOrDefault(u => u.Type == "UserId")?.Value ?? "", out userId);

            return userId;
        }
    }
}