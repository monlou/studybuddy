using System;
using System.Collections.Generic;

namespace StudyBuddy.Models
{
    public class CardDeck
    {
        public string ObjType { get; set; }
        public string Name { get; set; }
        public Uri CreatorAvatar { get; set; }
        public string CreatorName { get; set; }
        public List<Card> DeckContents { get; set; }
        public string Timestamp { get; set; }
    }
}
