

using ChatAPI.Models;

namespace ChatAPI.Interfaces
{
    public interface IGroupService
    {
        Task AddFriend(string userId, string friendUsername);
        Task CreateGroup(string userId, string groupName);
        Task RemoveFriend(string userName);
        Task JoinGroup(string userId, string groupName);
        Task<IEnumerable<Group>> GetUserGroups(string userId);
        Task<IEnumerable<User>> GetFriends(string friendsUsername);
    }
}
