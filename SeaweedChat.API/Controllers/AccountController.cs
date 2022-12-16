using SeaweedChat.Domain.Aggregates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using SeaweedChat.API.Models;
namespace SeaweedChat.API.Controllers;

[Route("api/account")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAccountRepository _repository;
    private readonly IPasswordEncoder _encoder;
    public AccountController(
        IAccountRepository repository,
        IPasswordEncoder encoder
    )
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _encoder = encoder;
    }

    [HttpPut]
    public async Task<ActionResult<bool>> AddAccount(AddAccountRequest request)
    {
        if (_repository.HasAccount(request.Email))
            return BadRequest("Account already exist");
        
        try 
        {
            await _repository.Add(new Account()
            {
                Email = request.Email,
                Password = request.Password,
                User = new User()
                {
                    Username = request.Username
                }
            });
            return true;
        }
        catch
        {
            return false;
        }
    }
}