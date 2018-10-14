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
	public class ChatPageViewModel : BindableBase, INotifyPropertyChanged
	{

        //private INavigationService _navigationService;
        //public EventHandler EditorCompleted { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public System.Windows.Input.ICommand EditorFABCommand { get; protected set; }

        private string _input;
        public string Input
        {
            get { return _input; }
            set
            {
                _input = value;
                OnPropertyChanged(nameof(Input));
            }
        }



        public ChatPageViewModel()
        {
            EditorFABCommand = new Command(SendInput);

        }

        public void SendInput()
        {
            Console.WriteLine("Hit SendInput");
            Console.WriteLine("Input is now: " + Input);
            ComposeMessage();
        }


        public void ComposeMessage()
        {
            Console.WriteLine("Hit ComposeMessage");

            Message message = new Message()
            {
                SenderID = 0,
                SenderName = "Me",
                Text = Input
            };
            Input = "";
            Console.WriteLine("Input is now: " + Input);


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
