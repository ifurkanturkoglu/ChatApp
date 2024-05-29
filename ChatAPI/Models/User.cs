using Microsoft.AspNetCore.Identity;

namespace ChatAPI.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }        
        public DateTime UserCreatedTime { get; set; }
        public ICollection<UserFriend>? Friends { get; set; }
        public ICollection<UserGroup>? UserGroups { get; set; }
        public ICollection<Message>? Messages { get; set; }
    }
}
