using SeaweedChat.Domain.Aggregates;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using SeaweedChat.API.Models;
using SeaweedChat.API.Security; 

namespace SeaweedChat.API.Controllers;

[Route("api/Auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAccountRepository _accRepository;
    private readonly IConfiguration _config;
    private readonly ILogger<AuthController> _logger;
    private readonly IPasswordEncoder? _encoder;
    private readonly IAuthentication _auth;
    public AuthController(
        IAccountRepository accRepository,
        IConfiguration config,
        ILogger<AuthController> logger,
        IAuthentication auth,
        IPasswordEncoder? encoder
    )
    {
        _accRepository = accRepository ?? throw new ArgumentNullException(nameof(accRepository));
        _config = config;
        _logger = logger;
        _encoder = encoder;
        _auth = auth;
    }

    [HttpGet("Token")]
    public async Task<ActionResult<AddAccountResponse>> GetAuthToken([FromQuery] GetTokenRequest request)
    {
        _logger.LogInformation($"{Request.HttpContext.Connection.RemoteIpAddress} requests JWT-Token");

        var account = await _accRepository.GetByEmail(request.Email);
        if (account == null)
            return BadRequest(new GetTokenResponse
            {
                Message = "Account with the email doesn't exist"
            });
        if (!account.VerifyPassword(_encoder?.Encode(request.Password) ?? request.Password))
            return BadRequest(new GetTokenResponse
            {
                Message = "Incorrect password"
            });

        var claims = new List<Claim>
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, account.Id.ToString()),
            new Claim("UserId", account.User.Id.ToString())
        };
        var jwt = _auth.Authenticate(claims);

        return Ok(new GetTokenResponse
        {
            Result = true,
            Message = "JWT-Token was successfully created",
            AccessToken = jwt
        });
    }
}