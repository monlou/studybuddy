using System;

namespace StudyBuddy.Models
{
    public class Message
    {
        public string ObjType { get; set; }
        public int SenderID { get; set; }
        public string SenderName { get; set; }
        public string Text { get; set; }
    }
}