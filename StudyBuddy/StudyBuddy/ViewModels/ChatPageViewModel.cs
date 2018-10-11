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
	public class ChatPageViewModel : BindableBase 
	{

        //private INavigationService _navigationService;
        //public EventHandler EditorCompleted { get; set; }
        public System.Windows.Input.ICommand EditorCompletedCommand { get; protected set; }


        private string _message = null;
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                RaisePropertyChanged(nameof(Message));
            }
        }


        public ChatPageViewModel()
        {
            EditorCompletedCommand = new Command(_ => SendEditorMessage(_));

        }

        private void SendEditorMessage(object param)
        {
            Console.WriteLine("AHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHH");
            Console.WriteLine(param);

        }
    }
}
