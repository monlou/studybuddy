using System;
namespace StudyBuddy.Models
{
    public class Card
    {
        public string ObjType { get; set; }
        public Uri CreatorAvatar { get; set; }
        public string CreatorName { get; set; }
        public string QuestionText { get; set; }
        public string CorrectText { get; set; }
        public string WrongTextOne { get; set; }
        public string WrongTextTwo { get; set; }
        public string Timestamp { get; set; }

    }
}
