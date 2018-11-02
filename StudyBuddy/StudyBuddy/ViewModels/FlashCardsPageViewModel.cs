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

#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
namespace StudyBuddy.ViewModels
{
    public class FlashCardsPageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public DelegateCommand SaveCardCommand { get; protected set; }
        public DelegateCommand SaveDeckCommand { get; protected set; }

        private INavigationService _navigationService;

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

        public FlashCardsPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;

            SaveCardCommand = new DelegateCommand(SaveCard);
            SaveDeckCommand = new DelegateCommand(SaveDeck);

            tempDeck = new List<Card>();
        }

        private void ResetInputs()
        {
            QuestionInput = "";
            AnswerInput = "";
        }

        private void SaveCard()
        {
            Card flashcard = new Card()
            {
                QuestionText = QuestionInput,
                AnswerText = AnswerInput
            };

            ResetInputs();
            tempDeck.Add(flashcard);
        }

        private async void SaveDeck()
        {

            // If the user has not added any flashcards to the deck, or failed to assign it a name, it won't be published to the database.
            if (tempDeck.Count == 0 || DeckName == null)
            {
                return;
            }

            CardDeck deck = new CardDeck()
            {
                ObjType = "Card",
                Name = DeckName,
                CreatorAvatar = MainPageViewModel.CurrentGoogleAvatar,
                CreatorName = MainPageViewModel.CurrentGoogleUsername,
                Timestamp = DateTime.Now.Ticks.ToString(),
                DeckContents = tempDeck,
                Length = tempDeck.Count
            };

            await FlashDBService.UploadFlashCard(deck);
            await _navigationService.GoBackAsync();
        }

        // Boilerplate responsible for acknowledging changes between the two-way View/ViewModel binding.
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
