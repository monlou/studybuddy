using System;

namespace StudyBuddy.Models
{
    public class GoogleUser
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public Uri Picture { get; set; }
        public int Score { get; set; }
    }
}