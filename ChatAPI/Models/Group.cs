namespace ChatAPI.Models
{
    public class Group
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public ICollection<UserGroup> UserGroups { get; set; }
        public ICollection<Message>? Messages { get; set; }
    }
}
