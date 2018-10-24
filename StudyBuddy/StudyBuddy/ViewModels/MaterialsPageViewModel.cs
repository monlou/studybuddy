using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using Prism.Logging;
using Prism.Services;
using System.Collections.ObjectModel;
using StudyBuddy.Models;
using StudyBuddy.Services;
using Xamarin.Forms;

namespace StudyBuddy.ViewModels
{
    public class MaterialsPageViewModel : ViewModelBase
    {
        private INavigationService _navigationService;
        public ObservableCollection<CardDeck> LoadedFlashcards { get; } = new ObservableCollection<CardDeck>();
        public DelegateCommand AddNewCardCommand { get; set; }


        public MaterialsPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            AddNewCardCommand = new DelegateCommand(AddNewCard);
            FlashDBService.FlashcardReceived += ChatClient_FlashcardReceived;


            _navigationService = navigationService;
        }

        private void ChatClient_FlashcardReceived(CardDeck deck)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Console.WriteLine("HIT DEVICE INVOKE THING");
                this.LoadedFlashcards.Add(deck);
            });
        }

        public async void AddNewCard()
        {
            await _navigationService.NavigateAsync("FlashCardsPage");
        }
    }
}
