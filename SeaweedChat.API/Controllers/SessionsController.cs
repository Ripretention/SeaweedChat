using SeaweedChat.Domain.Aggregates;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using SeaweedChat.API.Models;
using Microsoft.AspNetCore.Authorization;
using SeaweedChat.API.Security; 

namespace SeaweedChat.API.Controllers;

[Route("api/v1/Accounts/{AccountId:guid}/[controller]")]
public class SessionsController : ApiController
{
    private readonly IAccountRepository _accRepository;
    private readonly ISessionRepository _sessionRepository;
    private readonly IPasswordEncoder? _encoder;
    private readonly IAuthentication _auth;
    public SessionsController(
        IAccountRepository accRepository,
        IUserRepository userRepository,
        ISessionRepository sessionRepository,
        IAuthentication auth,
        IPasswordEncoder? encoder,
        ILogger<SessionsController>? logger
    ) : base(logger, userRepository)
    {
        _accRepository = accRepository ?? throw new ArgumentNullException(nameof(accRepository));
        _sessionRepository = sessionRepository ?? throw new ArgumentNullException(nameof(sessionRepository));
        _auth = auth;
        _encoder = encoder;
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
            Message = "Success",
            Sessions = sessions
        });
    }

    [HttpPut]
    [ProducesResponseType(201)]
    public async Task<ActionResult<AddSessionRequest>> AddSession([FromBody] AddSessionRequest request)
    {
        var account = await _accRepository.GetByEmail(request.Email);
        if (account == null)
            return BadRequest("Account with the email doesn't exist");
        if (!account.VerifyPassword(_encoder?.Encode(request.Password) ?? request.Password))
            return BadRequest("Incorrect password");

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

        return Created(
            CurrentRequestUri + $"/{session.Id}",
            new AddSessionResponse
            {
                Message = $"Session #{session.Id} successfully created",
                SessionToken = jwt
            }
        );
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
            return BadRequest("Invalid token");

        return Ok(new DeleteSessionResponse
        {
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
            return BadRequest("Invalid account");
        foreach (var session in sessions)
            await _sessionRepository.Remove(session);

        return Ok(new DeleteSessionResponse
        {
            Message = $"All sessions successfully deleted",
        });
    }
}