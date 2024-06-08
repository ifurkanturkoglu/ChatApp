using ChatAPI.Interfaces;
using ChatAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatAPI.Services
{
    public class MessageService : IMessageService
    {
        private readonly ChatDbContext context;

        public MessageService(ChatDbContext _context)
        {
            context = _context;
        }

        public async Task<IEnumerable<Message>> GetMessageByGroupAsync(int gropuId)
        {
            return await context.Messages.Where(a => a.GroupId == gropuId).OrderBy(a => a.MessageTime).ToListAsync();
        }

        public async Task SaveMessageAsync(Message message)
        {
            context.Messages.Add(message);
            await context.SaveChangesAsync();
        }


    }

}
