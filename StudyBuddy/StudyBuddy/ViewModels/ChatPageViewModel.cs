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
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using StudyBuddy.Helpers;
using Microsoft.Azure.Documents.Client;
using System.Collections.Generic;
using StudyBuddy.Services;
using System.Threading.Tasks;

#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
namespace StudyBuddy.ViewModels
{
	public class ChatPageViewModel : BindableBase, INotifyPropertyChanged
	{
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
            EditorFABCommand = new Command(ComposeMessage);
        }

        public async void ComposeMessage()
        {
            Console.WriteLine("Hit ComposeMessage");
            ChatDBService chat = new ChatDBService();


            Message message = new Message()
            {
                SenderID = 2,
                SenderName = "Google User",
                Text = Input
            };
            Input = "";
            Console.WriteLine("Input is now: " + Input);
            await chat.UploadMessage(message);

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
