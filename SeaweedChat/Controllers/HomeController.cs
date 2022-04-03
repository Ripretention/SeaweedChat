using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SeaweedChat.Infrastructure;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SeaweedChat.Models;

namespace SeaweedChat.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationContext context;

        public HomeController(ApplicationContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                long userId;
                return long.TryParse(User.Identity.Name, out userId)
                    ? View(await context.Users.FindAsync(userId))
                    : View();
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
