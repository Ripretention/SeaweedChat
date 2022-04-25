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
            ViewData["UserName"] = user.Username;

            var model = getUserChatPreviews(user);
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<long?> SendMessage([FromForm] MessageParams @params)
        {
            var user = await context.Users.FindAsync(long.Parse(User.Identity.Name));
            var chat = user?.Chats.FirstOrDefault(chat => chat.Id == @params.ChatId);
            if (user == null || chat == null)
                return null;

            var message = await context.Messages.AddAsync(new Message()
            {
                Date = DateTime.Now,
                isReaded = false,
                Owner = user,
                Text = @params.Text
            });

            await context.SaveChangesAsync();
            return message.Entity.Id;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Chat(long id)
        {
            var user = await context.Users.FindAsync(long.Parse(User.Identity.Name));
            var chat = user.Chats?.FirstOrDefault(chat => chat.Id == id);
            if (user == null || chat == null)
                return NotFound();
            ViewData["UserName"] = user.Username;

            Models.ChatModel model = new Models.ChatModel
            {
                Id = id,
                Messages = getChatLastMessages(chat),
                Members = chat.Members,
                ChatPreviews = getUserChatPreviews(user),
                Title = (await context.Users.FindAsync(id))?.Username ?? "Empty Chat"
            };

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> RedirectToChat(string username)
        {
            var currentUser = await context.Users.FindAsync(long.Parse(User.Identity.Name));
            var user = context.Users.FirstOrDefault(usr => usr.Username == username);
            if (currentUser == null || user == null || currentUser == user)
                return RedirectToAction("Index", "Messages");

            long chatId;
            var chat = currentUser?.Chats?.FirstOrDefault(chat => chat.Members.Count() == 2 && chat.Members.Contains(user));
            if (chat == null)
            {
                var createdChat = await context.Chats.AddAsync(new Chat
                {
                    Members = new[] { currentUser, user },
                    Messages = null
                });

                await context.SaveChangesAsync();
                chatId = createdChat.Entity.Id;
            }
            else
                chatId = chat.Id;

            return RedirectToAction("Chat", "Messages", new { Id = chatId.ToString() });
        }

        private IEnumerable<Models.ChatPreviewModel> getUserChatPreviews(User user)
        {
            var model = new List<Models.ChatPreviewModel>();
            foreach (var chat in user.Chats ?? Array.Empty<Chat>())
            {
                var lastMessage = chat.Messages.OrderByDescending(msg => msg.Date).First();
                model.Add(new Models.ChatPreviewModel
                {
                    Id = chat.Id,
                    LastMessage = TimeSpan.FromSeconds((DateTime.Now - lastMessage.Date).Seconds),
                    Text = lastMessage.Text.Substring(0, 512).Trim(),
                    isSelected = false
                });
            }
            return model;
        }

        public IEnumerable<Message> getChatLastMessages(Chat chat, int count = 50, int offset = 0) => context.Messages
            .Where(msg => msg.Chat == chat)
            .OrderByDescending(msg => msg.Date)
            .Skip(Math.Abs(offset))
            .Take(Math.Abs(count));
    }

    public class MessageParams
    {
        public string Text { get; set; }
        public long ChatId { get; set; }
    }
}
