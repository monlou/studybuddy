﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using Prism.Logging;
using Prism.Services;
using Prism.Mvvm;
using StudyBuddy.Services.Contracts;
using StudyBuddy.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using StudyBuddy.Services;

#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
namespace StudyBuddy.ViewModels
{
    public class FlashCardsPageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private INavigationService _navigationService;
        public event PropertyChangedEventHandler PropertyChanged;
        public System.Windows.Input.ICommand SaveCardCommand { get; protected set; }
        public System.Windows.Input.ICommand SaveDeckCommand { get; protected set; }
        public List<Card> tempDeck;

        private string _deckName;
        public string DeckName
        {
            get { return _deckName; }
            set
            {
                _deckName = value;
                OnPropertyChanged(nameof(DeckName));
            }
        }

        private string _questionInput;
        public string QuestionInput
        {
            get { return _questionInput; }
            set
            {
                _questionInput = value;
                OnPropertyChanged(nameof(QuestionInput));
            }
        }

        private string _answerInput;
        public string AnswerInput
        {
            get { return _answerInput; }
            set
            {
                _answerInput = value;
                OnPropertyChanged(nameof(AnswerInput));
            }
        }

        private string _wrong1Input;
        public string Wrong1Input
        {
            get { return _wrong1Input; }
            set
            {
                _wrong1Input = value;
                OnPropertyChanged(nameof(Wrong1Input));
            }
        }

        private string _wrong2Input;
        public string Wrong2Input
        {
            get { return _wrong2Input; }
            set
            {
                _wrong2Input = value;
                OnPropertyChanged(nameof(Wrong2Input));
            }
        }

        public FlashCardsPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            SaveCardCommand = new Command(SaveCard);
            SaveDeckCommand = new Command(SaveDeck);
            tempDeck = new List<Card>();

            _navigationService = navigationService;
        }

        public void ResetInputs()
        {
            QuestionInput = "";
            AnswerInput = "";
            Wrong1Input = "";
            Wrong2Input = "";
        }

        public void SaveCard()
        {
            Card flashcard = new Card()
            {
                QuestionText = QuestionInput,
                CorrectText = AnswerInput,
                WrongTextOne = Wrong1Input,
                WrongTextTwo = Wrong2Input
            };

            ResetInputs();
            tempDeck.Add(flashcard);
        }

        private async void SaveDeck()
        {
            CardDeck deck = new CardDeck()
            {
                ObjType = "Card",
                Name = DeckName,
                CreatorAvatar = MainPageViewModel.CurrentGoogleAvatar,
                CreatorName = MainPageViewModel.CurrentGoogleUsername,
                Timestamp = DateTime.Now.Ticks.ToString(),
                DeckContents = tempDeck
            };

            await FlashDBService.UploadFlashCard(deck);
            await _navigationService.GoBackAsync();
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var handler = PropertyChanged;
            if (handler != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
#pragma warning restore CS0114 // Member hides inherited member; missing override keyword
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
