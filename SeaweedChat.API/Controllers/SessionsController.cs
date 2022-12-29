using SeaweedChat.Domain.Aggregates;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using SeaweedChat.API.Models;
using Microsoft.AspNetCore.Authorization;
using SeaweedChat.API.Security; 

namespace SeaweedChat.API.Controllers;

[Route("api/v1/Accounts/{AccountId:guid}/[controller]")]
[ApiController]
public class SessionsController : ControllerBase
{
    private readonly IAccountRepository _accRepository;
    private readonly ISessionRepository _sessionRepository;
    private readonly ILogger<SessionsController> _logger;
    private readonly IPasswordEncoder? _encoder;
    private readonly IAuthentication _auth;
    public SessionsController(
        IAccountRepository accRepository,
        ISessionRepository sessionRepository,
        IAuthentication auth,
        IPasswordEncoder? encoder,
        ILogger<SessionsController> logger
    )
    {
        _accRepository = accRepository ?? throw new ArgumentNullException(nameof(accRepository));
        _sessionRepository = sessionRepository ?? throw new ArgumentNullException(nameof(sessionRepository));
        _auth = auth;
        _encoder = encoder;
        _logger = logger;
    }

    public Guid CurrentAccountId
    {
        get 
        {
            Guid.TryParse((string?)RouteData.Values.FirstOrDefault(v => v.Key == "AccountId").Value ?? "", out Guid accountId);
            return accountId;
        }
    }
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<GetSessionsResponse>> GetAllSessions()
    {
        var account = await _accRepository.Get(CurrentAccountId);
        var sessions = await _sessionRepository.GetAllAccountSessions(account);
        
        return Ok(new GetSessionsResponse
        {
            Result = true,
            Message = "Success",
            Sessions = sessions
        });
    }

    [HttpPut]
    public async Task<ActionResult<AddSessionRequest>> AddSession([FromBody] AddSessionRequest request)
    {
        var account = await _accRepository.GetByEmail(request.Email);
        if (account == null)
            return BadRequest(new AddSessionResponse
            {
                Message = "Account with the email doesn't exist"
            });
        if (!account.VerifyPassword(_encoder?.Encode(request.Password) ?? request.Password))
            return BadRequest(new AddSessionResponse
            {
                Message = "Incorrect password"
            });

        var claims = new List<Claim>
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, account.Id.ToString()),
            new Claim("UserId", account.User.Id.ToString())
        };
        var jwt = _auth.Authenticate(claims);

        var session = await _sessionRepository.Add(new Session 
        {
            Account = account,
            Token = jwt,
            Date = DateTime.Now
        });

        return Ok(new AddSessionResponse
        {
            Result = true,
            Message = $"Session #{session.Id} successfully created",
            SessionToken = jwt
        });
    }

    [Authorize]
    [HttpDelete("{sessionId:guid}")]
    public async Task<ActionResult<DeleteSessionResponse>> DeleteSession([FromRoute] Guid sessionId)
    {
        var session = await _sessionRepository.Get(sessionId);
        var accessToken = Request.Headers["Authorization"]
            .ToString()
            .Replace("Bearer ", "");
        
        if (session?.Account?.Id != CurrentAccountId || session?.Token != accessToken)
            return BadRequest(new DeleteSessionResponse 
            {
                Message = "Invalid token"
            });

        return Ok(new DeleteSessionResponse
        {
            Result = true,
            Message = $"Session #{session.Id} successfully deleted",
        });
    }
    [Authorize]
    [HttpDelete]
    public async Task<ActionResult<DeleteSessionResponse>> DeleteAllSessions()
    {
        var account = await _accRepository.Get(CurrentAccountId);
        var sessions = await _sessionRepository.GetAllAccountSessions(account);
        if (sessions.Count == 0)
            return BadRequest(new DeleteSessionResponse 
            {
                Message = "Invalid account"
            });
        foreach (var session in sessions)
            await _sessionRepository.Remove(session);

        return Ok(new DeleteSessionResponse
        {
            Result = true,
            Message = $"All sessions successfully deleted",
        });
    }
}