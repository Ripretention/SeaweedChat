using System;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using SeaweedChat.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;


namespace SeaweedChat.Controllers
{
    public class MessagesController : Controller
    {
        private ApplicationContext context;
        public MessagesController(ApplicationContext context)
        {
            this.context = context;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            long userId;
            Infrastructure.Models.User user = null;
            if (User.Identity.IsAuthenticated && long.TryParse(User.Identity.Name, out userId))
            {
                user = await context.Users.FindAsync(userId);
                if (user == null) return View();
            }

            ViewData["UserName"] = user.Username;
            return View();
        }
    }
}
