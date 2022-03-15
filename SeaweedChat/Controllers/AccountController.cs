using System;
using System.Linq;
using SeaweedChat.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SeaweedChat.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace SeaweedChat.Controllers
{
    public class AccountController : Controller
    {
        private ILogger<AccountController> logger;
        private ApplicationContext context;
        public AccountController(ILogger<AccountController> logger, ApplicationContext context)
        {
            this.logger = logger;
            this.context = context;
        }
            
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login([FromForm] LoginModel login)
        {
            logger.LogInformation($"User <{login.Username}> is logging...");
            if (ModelState.IsValid)
            {
                logger.LogInformation($"User <{login.Username}> has been logged");
                return RedirectToAction("Index", "Home");
            }

            return View(login);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([FromForm] RegisterModel reg)
        {
            logger.LogInformation($"User <{reg.Username}|{reg.Email}> is registering");

            if (context.Users.Any(usr => usr.Email == reg.Email))
                ModelState.AddModelError("Email", "User with the same email already registered");
            if (context.Users.Any(usr => usr.Username == reg.Username))
                ModelState.AddModelError("Username", "User with the same username already registered");

            if (ModelState.IsValid)
            {
                var user = await context.Users.AddAsync(new Infrastructure.Models.User
                {
                    Email = reg.Email,
                    Password = reg.Password,
                    Username = reg.Username
                });
                await context.SaveChangesAsync();

                logger.LogInformation($"User <{user.Entity.Id}|{user.Entity.Username}> has been registered");
                return RedirectToAction("Index", "Home");
            }

            return View(reg);
        }
    }
}
