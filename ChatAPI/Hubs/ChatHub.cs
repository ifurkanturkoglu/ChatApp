using ChatAPI.Interfaces;
using ChatAPI.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ChatAPI.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ChatDbContext context;

        private readonly IMessageService messageService;

        public ChatHub(ChatDbContext _context,IMessageService _messageService)
        {
            context = _context;
            messageService = _messageService;
        }

        public async Task AddFriend(string username)
        {
            try
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"{username}");
                //context.Groups.Add()
                Group newGroup = new Group
                {
                    //BURDA KALDIN message Service den ayar
                };

                await Clients.Caller.SendAsync("ReceiveMessage", $"{username} adlı kullanıcı arkadaş listesine eklendi.");
            }
            catch {
                await Clients.Caller.SendAsync("ReceiveMessage", "Ekleme başarısız oldu tekrar deneyin.");
            }
        }

        public async Task SendFriendMessage(string username,string msg)
        {
            
        }
    }
}
