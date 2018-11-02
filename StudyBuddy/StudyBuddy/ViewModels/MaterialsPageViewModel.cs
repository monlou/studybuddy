using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using StudyBuddy.Models;
using StudyBuddy.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace StudyBuddy.ViewModels
{
    public class MaterialsPageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public DelegateCommand AddNewCardCommand { get; set; }
        public DelegateCommand StartQuizCommand { get; set; }

        public ObservableCollection<CardDeck> LoadedFlashcards { get; } = new ObservableCollection<CardDeck>();

        private INavigationService _navigationService;

        private List<CardDeck> _loadedFlashcards;

        public CardDeck _selectedFlashcardDeck;
        public CardDeck SelectedFlashcardDeck
        {
            get
            {
                return _selectedFlashcardDeck;
            }

            set
            {
                _selectedFlashcardDeck = value;
                OnPropertyChanged(nameof(SelectedFlashcardDeck));

            }
        }

        public MaterialsPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;

            AddNewCardCommand = new DelegateCommand(AddNewCard);
            StartQuizCommand = new DelegateCommand(StartQuiz);

            _loadedFlashcards = new List<CardDeck>();
            LoadFlashcards();

            FlashDBService.FlashcardReceived += ChatClient_FlashcardReceived;

        }

        public async void StartQuiz()
        {

            if (SelectedFlashcardDeck == null)
            {
                return;
            }

            await _navigationService.NavigateAsync("FlashCardQuizPage");

            // Publishes the SelectedFlashcardDeck into Prism's EventAggregator store. A ViewModel subscribing
            // to this event will receive SelectedFlashcardDeck upon construction.
            MainPageViewModel.events.GetEvent<QuizEvent>().Publish(SelectedFlashcardDeck);
        }

        // When the dependency injection is fulfilled, the Observable Collection, which is
        // bound to the View, receives the incoming CardDeck into its properties.
        private void ChatClient_FlashcardReceived(CardDeck deck)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                this.LoadedFlashcards.Add(deck);
            });
        }

        public async void AddNewCard()
        {
            await _navigationService.NavigateAsync("FlashCardsPage");
        }

        // When the page is constructed, the client receives all pre-existing cards
        // from the database. These cards are loaded into a temporary list which are
        // then read into the ObservableCollection. This intermediary step was made 
        // necessary due to the fact that ObservableCollections cannot be assigned to Lists.
        private async void LoadFlashcards()
        {
            _loadedFlashcards = await FlashDBService.LoadFlashcards();

            foreach (var message in _loadedFlashcards)
            {
                this.LoadedFlashcards.Add(message);
            }
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
