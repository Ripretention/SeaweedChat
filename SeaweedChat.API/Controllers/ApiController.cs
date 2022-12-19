using Microsoft.AspNetCore.Mvc;
using SeaweedChat.Domain.Aggregates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
namespace SeaweedChat.API.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
public class ApiController : ControllerBase
{
    protected readonly ILogger<ChatController>? _logger;
    protected readonly IUserRepository _usrRepository;
    public ApiController(
        ILogger<ChatController> logger,
        IUserRepository usrRepository
    )
    {
        _usrRepository = usrRepository ?? throw new ArgumentNullException(nameof(usrRepository));
        _logger = logger;
    }

    protected Guid? CurrentUserId
    {
        get
        {
            Guid userId;
            Guid.TryParse(User.Claims.FirstOrDefault(u => u.Type == "UserId")?.Value ?? "", out userId);
            return userId == Guid.Empty
                ? null
                : userId;
        }
    }
}