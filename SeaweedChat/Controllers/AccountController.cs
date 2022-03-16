using System;
using SeaweedChat.Models;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using SeaweedChat.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

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
        public async Task<IActionResult> Login([FromForm] LoginModel login)
        {
            logger.LogInformation($"User <{login.Username}> is logging...");
            var user = await context.Users.FirstOrDefaultAsync(usr => usr.Username == login.Username);

            if (user == null)
                ModelState.AddModelError("Username", "User with such username hasn't registered");
            else if (user.Password != login.Password)
                ModelState.AddModelError("Password", "Wrong password. Try it again.");

            if (ModelState.IsValid)
            {
                logger.LogInformation($"User <{login.Username}> has been logged");
                await Authenticate(user.Id);
                return RedirectToAction("Index", "Home");
            }

            return View(login);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
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

            if (await context.Users.AnyAsync(usr => usr.Email == reg.Email))
                ModelState.AddModelError("Email", "User with the same email already registered");
            if (await context.Users.AnyAsync(usr => usr.Username == reg.Username))
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
                await Authenticate(user.Entity.Id);
                return RedirectToAction("Index", "Home");
            }

            return View(reg);
        }

        private async Task Authenticate(long userId)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userId.ToString())
            };

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}
