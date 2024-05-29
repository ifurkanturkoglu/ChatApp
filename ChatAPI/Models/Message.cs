namespace ChatAPI.Models
{
    public class Message
    {
        public int MessageId { get; set; }
        public string Text { get; set; }
        public DateTime MessageTime { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public int? GroupId { get; set; }
        public Group Group { get; set; }
    }
}
