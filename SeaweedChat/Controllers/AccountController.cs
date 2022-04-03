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
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Settings()
        {
            long userId;
            Infrastructure.Models.User user = null;
            if (User.Identity.IsAuthenticated && long.TryParse(User.Identity.Name, out userId))
            {
                user = await context.Users.FindAsync(userId);
                if (user == null) return View();
            }

            ViewData["UserName"] = user.Username;
            return View(new UpdateSettingsModel
            {
                Email = user.Email,
                Username = user.Username
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm] LoginModel login)
        {
            if (User.Identity.IsAuthenticated)
            {
                ModelState.AddModelError("Password", "You are already logged in");
                return View(login);
            }

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

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateSettings([FromForm] UpdateSettingsModel update)
        {
            long userId;
            Infrastructure.Models.User user;
            if (long.TryParse(User.Identity.Name, out userId))
                user = await context.Users.FindAsync(userId);
            else
                return RedirectToAction("Login", "Account");

            if (update.ConfirmPassword != null && user.Password != update.ConfirmPassword)
                ModelState.AddModelError("ConfirmPassword", "Wrong password. Try it again.");
            else if (update.Username != null && user.Username != update.Username && await context.Users.AnyAsync(usr => usr.Username == update.Username))
                ModelState.AddModelError("Username", "Username is occupied");
            else if (update.Email != null && user.Email != update.Email && await context.Users.AnyAsync(usr => usr.Email == update.Email))
                ModelState.AddModelError("Email", "Email is occupied");

            if (ModelState.IsValid)
            {
                if (update.Username != null && user.Username != update.Username)
                    user.Username = update.Username;
                if (update.Email != null && user.Email != update.Email)
                    user.Email = update.Email;
                if (update.Password != null && user.Password != update.Password)
                    user.Password = update.Password;

                await context.SaveChangesAsync();
            }

            return View("Settings", update);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([FromForm] RegisterModel reg)
        {
            if (User.Identity.IsAuthenticated)
            {
                ModelState.AddModelError("Email", "You are already logged in");
                return View(reg);
            }

            logger.LogInformation($"User <{reg.Username}|{reg.Email}> is registering");

            if (await context.Users.AnyAsync(usr => usr.Email == reg.Email))
                ModelState.AddModelError("Email", "User with the same email already registered");
            else if (await context.Users.AnyAsync(usr => usr.Username == reg.Username))
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
