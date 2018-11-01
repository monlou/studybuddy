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

#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
namespace StudyBuddy.ViewModels
{
    public class MaterialsPageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private INavigationService _navigationService;
        public event PropertyChangedEventHandler PropertyChanged;

        private List<CardDeck> _loadedFlashcards;
        public ObservableCollection<CardDeck> LoadedFlashcards { get; } = new ObservableCollection<CardDeck>();

        public DelegateCommand AddNewCardCommand { get; set; }
        public DelegateCommand StartQuizCommand { get; set; }




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
            MainPageViewModel.events.GetEvent<QuizEvent>().Publish(SelectedFlashcardDeck);


        }

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

        private async void LoadFlashcards()
        {
            _loadedFlashcards = await FlashDBService.LoadFlashcards();

            foreach (var message in _loadedFlashcards)
            {
                this.LoadedFlashcards.Add(message);
            }
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
