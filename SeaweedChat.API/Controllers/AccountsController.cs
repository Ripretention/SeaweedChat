using SeaweedChat.Domain.Aggregates;
using Microsoft.AspNetCore.Mvc;
using SeaweedChat.API.Models;
using SeaweedChat.API.Security;
using Microsoft.AspNetCore.Authorization;
namespace SeaweedChat.API.Controllers;

[Route("api/v1/[controller]")]
public class AccountsController : ApiController
{
    private readonly IAccountRepository _accRepository;
    private readonly IPasswordEncoder? _encoder;
    public AccountsController(
        IAccountRepository accRepository,
        IUserRepository usrRepostiroy,
        IPasswordEncoder? encoder,
        ILogger<AccountsController>? logger
    ) : base(logger, usrRepostiroy)
    {
        _accRepository = accRepository ?? throw new ArgumentNullException(nameof(accRepository));
        _encoder = encoder;
    }

    [AllowAnonymous]
    [HttpPut]
    [ProducesResponseType(201)]
    public async Task<ActionResult<AddAccountResponse>> AddAccount([FromBody] AddAccountRequest request)
    {
        try 
        {
            var user = await _usrRepository.Add(new User()
            {
                Username = request.Username
            });
            var account = await _accRepository.Add(new Account()
            {
                Email = request.Email,
                Password = _encoder?.Encode(request.Password) ?? request.Password,
                User = user
            });
            
            return Created(
                CurrentRequestUri + $"/{account.Id}",
                new AddAccountResponse
                {
                    Message = $"Account #{account.Id} successfully added"
                }
            );
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("{accountId:guid}")]
    public async Task<ActionResult<DeleteAccountResponse>> DeleteAccount([FromRoute] Guid accountId, [FromQuery] string password)
    {
        var account = await _accRepository.Get(accountId);
        if (!(account?.VerifyPassword(_encoder?.Encode(password) ?? password) ?? false))
            return BadRequest("Incorrect password");

        await _accRepository.Remove(account);
        await _usrRepository.Remove(account.User);
        return Ok(new DeleteAccountResponse
        {
            Message = $"Account #{account.Id} successfully deleted"
        });
    }

    [HttpPost("{accountId:guid}")]
    public async Task<ActionResult<EditAccountResponse>> EditAccount([FromRoute] Guid accountId, [FromBody] EditAccountRequest request)
    {
        var account = await _accRepository.Get(accountId);
        if (!(account?.VerifyPassword(_encoder?.Encode(request.ConfirmationPassword) ?? request.ConfirmationPassword) ?? false))
            return BadRequest("Incorrect confirmation password");

        var updates = new List<string>();
        if (request.Password != null)
        {
            var encodedPassword = _encoder?.Encode(request.Password) ?? request.Password;
            if (!account.VerifyPassword(encodedPassword))
            {
                account.Password = encodedPassword;;
                updates.Add("password");
            }
        }
        if (request.Email != null && request.Email != account.Email)
        {
            account.Email = request.Email;
            updates.Add("email");
        }
        if (request.Username != null && request.Username != account.User.Username)
        {
            account.User.Username = request.Username;
            updates.Add("username");
        }

        _logger?.LogInformation($"edit {account}: {string.Join(", ", updates)}");
        await _accRepository.Update();

        return Ok(new EditAccountResponse
        {
            Message = $"Fields {string.Join(", ", updates)} successfully edited"
        });
    }
}