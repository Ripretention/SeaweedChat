using Microsoft.AspNetCore.Mvc;
using SeaweedChat.Domain.Aggregates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
namespace SeaweedChat.API.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
public class ApiController : ControllerBase
{
    protected readonly ILogger? _logger;
    protected readonly IUserRepository _usrRepository;
    public ApiController(
        ILogger? logger,
        IUserRepository usrRepository
    )
    {
        _usrRepository = usrRepository ?? throw new ArgumentNullException(nameof(usrRepository));
        _logger = logger;
    }

    protected Guid CurrentUserId
    {
        get
        {
            Guid.TryParse(User.Claims.FirstOrDefault(u => u.Type == "UserId")?.Value ?? "", out Guid userId);
            return userId;
        }        
    }
    protected string CurrentRequestUri
    {
        get => $"{Request.Scheme}://{Request.Host}{Request.PathBase}{Request.Path}{Request.QueryString}";
    }
}