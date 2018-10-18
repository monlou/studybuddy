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

#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
namespace StudyBuddy.ViewModels
{
	public class ChatPageViewModel : BindableBase, INotifyPropertyChanged
	{
        //DocumentClient ChatClient = new DocumentClient(new Uri(Keys.CosmosDBUri), Keys.CosmosDBUri);
        //Uri collectionLink = UriFactory.CreateDocumentCollectionUri("Chat", "Messages");




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

        public void ComposeMessage()
        {
            Console.WriteLine("Hit ComposeMessage");

            Message message = new Message()
            {
                SenderID = 0,
                SenderName = "Google User",
                Text = Input
            };
            Input = "";
            Console.WriteLine("Input is now: " + Input);
            //UploadMessage(message);
        }


        /*public async void UploadMessage(Message message)
        {
            Console.WriteLine("Hit UploadMessage");
            await ChatClient.CreateDocumentAsync(collectionLink, message);
            Console.WriteLine("Created a new message in " + collectionLink);

        }*/


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
