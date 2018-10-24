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

        public async void MaterialsNavigate()
        {
            await _navigationService.NavigateAsync("MaterialsPage");
        }

        public async void SaveCard()
        {
            Card flashcard = new Card()
            {
                CreatorID = 0,
                CreatorName = "Test",
                QuestionText = "Powerhouse of the cell",
                CorrectText = "Mitochondria",
                WrongTextOne = "Nucleus",
                WrongTextTwo = "Chromatin"
            };

            Input = "";
            await FlashDBService.UploadFlashCard(flashcard);
            MaterialsNavigate(); 
        }

        private void OnChangedProperty([CallerMemberName] string propertyName = "")
        {
            var handler = ChangedProperty;
            if (handler != null)
                ChangedProperty(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
