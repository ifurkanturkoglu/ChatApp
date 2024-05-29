using ChatAPI.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ChatAPI.Hubs
{
    public class ChatHub : Hub
    {
        public  ChatDbContext context;

        public ChatHub(ChatDbContext _context)
        {
            context = _context;
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
