using ChatAPI.Models;

namespace ChatAPI.Interfaces
{
    public interface IMessageService
    {
        Task SaveMessageAsync(Message message);

        Task<IEnumerable<Message>> GetMessageByGroupAsync(int gropuId);
    }
}
