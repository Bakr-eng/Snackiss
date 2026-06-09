using System;


namespace Snackis.Domain.Entities
{
    public class PrivateMessage
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
    }
}
