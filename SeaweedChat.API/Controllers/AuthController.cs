using SeaweedChat.Domain.Aggregates;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using SeaweedChat.API.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;    

namespace SeaweedChat.API.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAccountRepository _accRepository;
    private readonly IConfiguration _config;
    private readonly ILogger<AuthController> _logger;
    public AuthController(
        IAccountRepository accRepository,
        IConfiguration config,
        ILogger<AuthController> logger
    )
    {
        _accRepository = accRepository ?? throw new ArgumentNullException(nameof(accRepository));
        _config = config;
        _logger = logger;
    }

    [HttpGet("token")]
    public async Task<ActionResult<AddAccountResponse>> GetAuthToken(GetTokenRequest request)
    {
        _logger.LogInformation($"{Request.HttpContext.Connection.RemoteIpAddress} requests JWT-Token");

        var account = await _accRepository.GetByEmail(request.Email);
        if (account == null)
            return BadRequest(new GetTokenResponse
            {
                Message = "Account with such email doesn't exist"
            });
        if (!account.VerifyPassword(request.Password))
            return BadRequest(new GetTokenResponse
            {
                Message = "Incorrect password"
            });

        var claims = new List<Claim>
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, account.Id.ToString()),
            new Claim("UserId", account.User.Id.ToString())
        };
        var claimsIdentity = new ClaimsIdentity(claims, "Token");

        var securityKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(_config["Jwt:Key"] ?? "the most secret key"));    
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);    

        var time = DateTime.UtcNow;
        var jwt = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            notBefore: time,
            expires: time.Add(TimeSpan.FromMinutes(_config.GetValue<double>("Jwt:Lifetime"))),
            claims: claimsIdentity.Claims,
            signingCredentials: credentials
        );

        return Ok(new GetTokenResponse
        {
            Result = true,
            Message = "JWT-Token was successfully created",
            AccessToken = new JwtSecurityTokenHandler().WriteToken(jwt)
        });
    }
}