using System;
namespace StudyBuddy.Models
{
    public class Card
    {
        public int CreatorID { get; set; }
        public string CreatorName { get; set; }
        public string QuestionText { get; set; }
        public string CorrectText { get; set; }
        public string WrongTextOne { get; set; }
        public string WrongTextTwo { get; set; }
    }
}
