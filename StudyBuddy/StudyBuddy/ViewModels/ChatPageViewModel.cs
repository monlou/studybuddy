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
using Microsoft.Azure.Documents.ChangeFeedProcessor;
using System.Collections.ObjectModel;

#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
namespace StudyBuddy.ViewModels
{
    public class ChatPageViewModel : BindableBase, INotifyPropertyChanged
	{
        public event PropertyChangedEventHandler PropertyChanged;
        public System.Windows.Input.ICommand EditorFABCommand { get; protected set; }
        public string CategoryInput = "Question";

        public ObservableCollection<Message> LoadedMessages { get; } = new ObservableCollection<Message>();

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
            ChatDBService.MessageReceived += ChatClient_MessageReceived;
        }

        private void ChatClient_MessageReceived(Message message)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                this.LoadedMessages.Add(message);
            });
        }

        void OnPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex != -1)
            {
                CategoryInput = picker.Items[selectedIndex];
            }
        }

        public async void ComposeMessage()
        {
            Message message = new Message()
            {
                ObjType = "Msg",
                SenderAvatar = MainPageViewModel.CurrentGoogleAvatar,
                SenderName = MainPageViewModel.CurrentGoogleUsername,
                Text = Input,
                Category = CategoryInput,
                Timestamp = DateTime.Now.Ticks.ToString()
            };
            Input = "";
            await ChatDBService.UploadMessage(message);
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
