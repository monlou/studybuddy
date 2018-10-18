using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using StudyBuddy.Services.Contracts;
using Prism.Services;
using StudyBuddy.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace StudyBuddy.ViewModels
{
	public class FlashCardsPageViewModel : BindableBase
	{
        public event PropertyChangedEventHandler ChangedProperty;
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

        public FlashCardsPageViewModel()
        {
            SaveCardCommand = new Command(SaveCard);
        }

        public void SaveCard()
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



        }

        private void OnChangedProperty([CallerMemberName] string propertyName = "")
        {
            var handler = ChangedProperty;
            if (handler != null)
                ChangedProperty(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
