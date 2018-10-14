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

#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
namespace StudyBuddy.ViewModels
{
	public class ChatPageViewModel : BindableBase 
	{

        //private INavigationService _navigationService;
        //public EventHandler EditorCompleted { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public System.Windows.Input.ICommand EditorCompletedCommand { get; protected set; }

        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }


        public ChatPageViewModel()
        {
            EditorCompletedCommand = new Command(SendEditorMessage);

        }

        public void SendEditorMessage()
        {
            Console.WriteLine("Hit SendEditorMessage");
            Console.WriteLine(Message);
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var handler = PropertyChanged;
            if (handler != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
#pragma warning restore CS0114 // Member hides inherited member; missing override keyword
