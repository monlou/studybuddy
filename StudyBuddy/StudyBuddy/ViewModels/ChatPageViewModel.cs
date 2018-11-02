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
        public ObservableCollection<Message> LoadedMessages { get; } = new ObservableCollection<Message>();

        public DelegateCommand EditorFABCommand { get; protected set; }

        private List<Message> _loadedMessages;

        private string _pickerCategory;
        public string PickerCategory
        {
            get { return _pickerCategory; }
            set
            {
                _pickerCategory = value;
                OnPropertyChanged(nameof(PickerCategory));
            }
        }

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
            _loadedMessages = new List<Message>();

            EditorFABCommand = new DelegateCommand(ComposeMessage);

            LoadMessages();
            ChatDBService.MessageReceived += ChatClient_MessageReceived;
        }

        // When the dependency injection is fulfilled, the Observable Collection, which is
        // bound to the View, receives the incoming Message into its properties.
        private void ChatClient_MessageReceived(Message message)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                this.LoadedMessages.Add(message);
            });
        }

        public async void ComposeMessage()
        {
            // Disallows the user from publishing empty strings.
            if (Input == null)
            {
                return;
            }

            Message message = new Message()
            {
                ObjType = "Msg",
                SenderAvatar = MainPageViewModel.CurrentGoogleAvatar,
                SenderName = MainPageViewModel.CurrentGoogleUsername,
                Text = Input,
                Category = PickerCategory,
                Timestamp = DateTime.Now.Ticks.ToString()
            };

            Input = "";
            await ChatDBService.UploadMessage(message);
        }

        // When the page is constructed, the client receives all pre-existing messages
        // from the database. These messages are loaded into a temporary list which are
        // then read into the ObservableCollection. This intermediary step was made 
        // necessary due to the fact that ObservableCollections cannot be assigned to Lists.
        private async void LoadMessages()
        {
            _loadedMessages = await ChatDBService.LoadMessages();

            foreach (var message in _loadedMessages)
            {
                this.LoadedMessages.Add(message);
            }
        }

        // Boilerplate responsible for acknowledging changes between the two-way View/ViewModel binding.
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
