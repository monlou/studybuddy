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
    public class FlashCardQuizPageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private INavigationService _navigationService;
        public event PropertyChangedEventHandler PropertyChanged;
        public static CardDeck QuizDeck;

        public FlashCardQuizPageViewModel(INavigationService navigationService) : base(navigationService)
        {

            QuizDeck = new CardDeck();
            _navigationService = navigationService;
        }


        public static void ReceiveQuizDeck(CardDeck deck)
        {
            QuizDeck = deck;
            Console.WriteLine(QuizDeck.Name.ToString());

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
