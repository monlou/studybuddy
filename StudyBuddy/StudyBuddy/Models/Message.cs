using System;

namespace StudyBuddy.Models
{
    public class Message
    {
        public string ObjType { get; set; }
        public Uri SenderAvatar { get; set; }
        public string SenderName { get; set; }
        public string Text { get; set; }
        public string Timestamp { get; set; }
    }
}