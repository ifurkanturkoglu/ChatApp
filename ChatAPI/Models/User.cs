using Microsoft.AspNetCore.Identity;

namespace ChatAPI.Models
{
    public class User : IdentityUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public ICollection<UserFriend>? Friends { get; set; }
        public ICollection<UserGroup>? UserGroups { get; set; }
        public ICollection<Message>? Messages { get; set; }
    }
}
