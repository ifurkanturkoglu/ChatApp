using ChatAPI.Hubs;
using ChatAPI.Interfaces;
using ChatAPI.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ChatAPI.Services
{
    public class GroupService : IGroupService
    {
        private readonly ChatDbContext context;

        public GroupService(ChatDbContext _context)
        {
            context = _context;
        }

        public async Task AddFriend(string userId,string friendUsername)
        {
            User friend = context.Users.Where(a => a.UserName == friendUsername).FirstOrDefault();


            if (friend == null)
            {
                throw new ArgumentException("Friend not found");
            }

            UserFriend friendRelation = new UserFriend {
                UserId = userId,
                FriendId = friend.Id
            };

            context.UserFriends.Add(friendRelation);
            await context.SaveChangesAsync();
        }


        public async Task CreateGroup(string userId,string groupName)
        {
            Group newGroup = new Group
            {
                GroupName = groupName                
            };

            context.Groups.Add(newGroup);

            await context.SaveChangesAsync();

            UserGroup newUserGroup = new UserGroup
            {
                GroupId = newGroup.GroupId,
                UserId = userId,
            };

            context.UserGroups.Add(newUserGroup);

            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Group>> GetUserGroups(string userId)
        {
            ICollection<Group> groups = await context.UserGroups
                .Include(a => a.Group)
                .Where(a => a.UserId == userId)
                .Select(a => a.Group)
                .ToListAsync();

            return groups;
        }

        public async Task<IEnumerable<User>> GetFriends(string userName)
        {
            ICollection<User> users = await context.UserFriends
                .Include(a => a.User)
                .Where(a => a.User.UserName == userName)
                .Select(a => a.Friend)
                .ToListAsync();

            return users;
        }

        public async Task JoinGroup(string userId,string groupName)
        {
            Group group = context.Groups.Where(a => a.GroupName ==  groupName).FirstOrDefault();

            if (group == null)
            {
                throw new ArgumentException("Group not found");
            }

            UserGroup userGroup = new UserGroup
            {
                UserId = userId,
                GroupId = group.GroupId
            };

            context.UserGroups.Add(userGroup);

            await context.SaveChangesAsync();
        }


        public async Task RemoveFriend(string userName)
        {
            User friend = context.Users.Where(a => a.UserName == userName).FirstOrDefault();

            UserFriend userRelation = context.UserFriends.Where(a => a.FriendId == friend.Id).FirstOrDefault();

            context.UserFriends.Remove(userRelation);

            await context.SaveChangesAsync();
        }

    }
}
