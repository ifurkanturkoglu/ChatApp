using ChatAPI.Interfaces;
using ChatAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ChatAPI.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        ChatDbContext context;

        IGroupService groupService;
        IMessageService messageService;

        public ChatController(ChatDbContext _context,IGroupService _groupService,IMessageService _messageService)
        {
            groupService = _groupService;
            messageService = _messageService;
            context = _context;
        }

        [HttpGet("groups")]
        public async Task<IActionResult> GetGroups()
        {
            string userId = context.Users
                .Where(a => a.UserName == User.FindFirstValue(ClaimTypes.NameIdentifier))
                .Select(a => a.Id)
                .Single();

            IEnumerable<Group> groups = await groupService.GetUserGroups(userId);
            return Ok(groups);
        }
        [HttpGet("friends")]
        public async Task<IActionResult> GetFriends()
        {
            string userId = context.Users
                .Where(a => a.UserName == User.FindFirstValue(ClaimTypes.NameIdentifier))
                .Select(a => a.Id)
                .Single();

            IEnumerable<User> groups = await groupService.GetFriends(userId);
            return Ok(groups);
        }

        [HttpGet("/messages/{groupname}")]
        public  async Task<IActionResult> GetChatMessage([FromRoute]string groupName)
        {
            int? groupId = context.Messages
                .Include(a => a.Group)
                .Where(a => a.Group.GroupName == groupName)
                .Select(a => a.GroupId)
                .SingleOrDefault();

            if(groupId == null)
            {
                throw new Exception("Group is not found");
            }

            IEnumerable<Message> messages = await messageService.GetMessageByGroupAsync((int)groupId);
            return Ok(messages);
        }

    }
}
