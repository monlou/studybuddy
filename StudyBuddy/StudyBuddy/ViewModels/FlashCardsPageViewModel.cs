using System;
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

namespace StudyBuddy.ViewModels
{
    public class FlashCardsPageViewModel : ViewModelBase
    {
        private INavigationService _navigationService;
        public event PropertyChangedEventHandler ChangedProperty;
        //public DelegateCommand AddNewCardCommand { get; set; }
        public System.Windows.Input.ICommand SaveCardCommand { get; protected set; }
        public System.Windows.Input.ICommand SaveDeckCommand { get; protected set; }
        public List<Card> tempDeck;

        private string _input;
        public string Input
        {
            get { return _input; }
            set
            {
                _input = value;
                OnChangedProperty(nameof(Input));
            }
        }

        public FlashCardsPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            //AddNewCardCommand = new DelegateCommand(AddNewCard);
            SaveCardCommand = new Command(SaveCard);
            SaveDeckCommand = new Command(SaveDeck);
            tempDeck = new List<Card>();


        _navigationService = navigationService;
        }

        public void SaveCard()
        {
            Card flashcard = new Card()
            {
                ObjType = "Card",
                CreatorAvatar = MainPageViewModel.CurrentGoogleAvatar,
                CreatorName = MainPageViewModel.CurrentGoogleUsername,
                QuestionText = "Powerhouse of the cell NUMBA 1",
                CorrectText = "Mitochondria",
                WrongTextOne = "Nucleus",
                WrongTextTwo = "Chromatin",
                Timestamp = DateTime.Now.Ticks.ToString()
            };

            tempDeck.Add(flashcard);
        }

        private async void SaveDeck()
        {
            CardDeck deck = new CardDeck()
            {
                ObjType = "Card",
                Name = "Biology 101",
                CreatorAvatar = MainPageViewModel.CurrentGoogleAvatar,
                CreatorName = MainPageViewModel.CurrentGoogleUsername,
                Timestamp = DateTime.Now.Ticks.ToString(),
                DeckContents = tempDeck
            };


            await FlashDBService.UploadFlashCard(deck);
            await _navigationService.GoBackAsync();
        }

        private void OnChangedProperty([CallerMemberName] string propertyName = "")
        {
            var handler = ChangedProperty;
            if (handler != null)
                ChangedProperty(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
