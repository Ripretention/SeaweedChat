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
            var user = await context.Users.FindAsync(long.Parse(User.Identity.Name));
            var lastMessages = getLastUserMessages(user);
            ViewData["UserName"] = user.Username;

            var model = constructChatPreviews(lastMessages);

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Chat(long id)
        {
            var user = await context.Users.FindAsync(long.Parse(User.Identity.Name));
            var lastMessages = getLastUserMessages(user, 40);
            ViewData["UserName"] = user.Username;

            var lastChatMessages = context.Messages
               .Where(msg => (msg.Owner.Id == user.Id && msg.Peer.Id == id) || (msg.Peer.Id == user.Id && msg.Owner.Id == id))
               .OrderByDescending(msg => msg.Date)
               .Take(50);

            Models.ChatModel model = new Models.ChatModel
            {
                Id = id,
                Messages = lastMessages,
                Members = lastChatMessages
                    .Select(msg => new[] { msg.Owner, msg.Peer })
                    .SelectMany(mbr => mbr)
                    .Distinct(),
                ChatPreviews = constructChatPreviews(lastMessages),
                Title = (await context.Users.FindAsync(id))?.Username ?? "Empty Chat"
            };

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public IActionResult RedirectToChat(string username)
        {
            var user = context.Users.FirstOrDefault(usr => usr.Username == username);
            return user == null
                ? RedirectToAction("Index", "Messages")
                : RedirectToAction("Chat", "Messages", new { Id = user.Id.ToString() });
        }

        private IEnumerable<Models.ChatPreviewModel> constructChatPreviews(IEnumerable<Message> lastMessages)
        {
            var model = new List<Models.ChatPreviewModel>();
            foreach (var message in lastMessages)
                model.Add(new Models.ChatPreviewModel
                {
                    Id = message.Owner.Id,
                    LastMessage = TimeSpan.FromSeconds(Math.Abs(DateTime.Now.Second - message.Date.Second)),
                    Text = message.Text.Substring(0, 512).Trim(),
                    isSelected = false
                });

            return model;
        }
        private IEnumerable<Message> getLastUserMessages(User user, int count = 10) =>
           context.Messages
               .Where(msg => msg.Peer.Id == user.Id || msg.Owner.Id == user.Id)
               .ToList()
               .GroupBy(msg => msg.Peer.Id == user.Id ? msg.Peer.Id : msg.Owner.Id)
               .Select(groupedMsg => groupedMsg.OrderByDescending(msg => msg.Date).FirstOrDefault())
               .Where(msg => msg != null)
               .OrderByDescending(msg => msg.Date)
               .Take(Math.Abs(count));
    }
}
