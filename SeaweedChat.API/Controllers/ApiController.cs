using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
namespace SeaweedChat.API.Controllers;

[Route("api/[controller]s")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
public class ApiController : ControllerBase
{
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