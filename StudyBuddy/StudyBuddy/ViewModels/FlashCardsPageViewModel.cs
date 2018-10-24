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

            _navigationService = navigationService;
        }

        public async void SaveCard()
        {
            Card flashcard1 = new Card()
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

            Card flashcard2 = new Card()
            {
                ObjType = "Card",
                CreatorAvatar = MainPageViewModel.CurrentGoogleAvatar,
                CreatorName = MainPageViewModel.CurrentGoogleUsername,
                QuestionText = "Powerhouse of the cell NUMBA 2",
                CorrectText = "Mitochondria",
                WrongTextOne = "Nucleus",
                WrongTextTwo = "Chromatin",
                Timestamp = DateTime.Now.Ticks.ToString()
            };

            CardDeck deck = new CardDeck()
            {
                ObjType = "Card",
                Name = "Biology 101",
                CreatorAvatar = MainPageViewModel.CurrentGoogleAvatar,
                CreatorName = MainPageViewModel.CurrentGoogleUsername,
                Timestamp = DateTime.Now.Ticks.ToString(),
                DeckContents = new List<Card>()
            };

            deck.DeckContents.Add(flashcard1);
            deck.DeckContents.Add(flashcard2);




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
