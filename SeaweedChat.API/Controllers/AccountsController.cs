using SeaweedChat.Domain.Aggregates;
using Microsoft.AspNetCore.Mvc;
using SeaweedChat.API.Models;
using SeaweedChat.API.Security;
namespace SeaweedChat.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly IAccountRepository _accRepository;
    private readonly IUserRepository _usrRepository;
    private readonly IPasswordEncoder? _encoder;
    public AccountsController(
        IAccountRepository accRepository,
        IUserRepository usrRepostiroy,
        IPasswordEncoder? encoder
    )
    {
        _accRepository = accRepository ?? throw new ArgumentNullException(nameof(accRepository));
        _usrRepository = usrRepostiroy ?? throw new ArgumentNullException(nameof(usrRepostiroy));
        _encoder = encoder;
    }

    [HttpPut]
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
            
            return Ok(new AddAccountResponse
            {
                Result = true,
                Message = $"Account #{account.Id} successfully added"
            });
        }
        catch (ArgumentException e)
        {
            return BadRequest(new AddAccountResponse
            {
                Result = false,
                Message = e.Message
            });
        }
    }
}