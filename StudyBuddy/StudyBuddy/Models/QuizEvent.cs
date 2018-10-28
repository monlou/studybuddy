using System;
using Prism.Events;

namespace StudyBuddy.Models
{
    public class QuizEvent : PubSubEvent<CardDeck> { }
}