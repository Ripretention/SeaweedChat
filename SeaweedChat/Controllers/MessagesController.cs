using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using SeaweedChat.Infrastructure;
using SeaweedChat.Infrastructure.Models;
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
            User user = await context.Users.FindAsync(long.Parse(User.Identity.Name));
            ViewData["UserName"] = user.Username;

            var lastMessages = context.Messages
                .Where(msg => msg.Peer.Id == user.Id)
                .ToList()
                .GroupBy(msg => msg.Peer.Id)
                .Select(groupedMsg => groupedMsg.OrderByDescending(msg => msg.Date).FirstOrDefault())
                .Where(msg => msg != null)
                .OrderByDescending(msg => msg.Date)
                .Take(10);

            var model = new List<Models.ChatPreviewModel>();
            foreach (var message in lastMessages)
                model.Add(new Models.ChatPreviewModel
                {
                    Id = message.Owner.Id,
                    LastMessage = TimeSpan.FromSeconds(Math.Abs(DateTime.Now.Second - message.Date.Second)),
                    Text = message.Text.Substring(0, 512).Trim(),
                    isSelected = false
                });

            return View(model);
        }
    }
}
