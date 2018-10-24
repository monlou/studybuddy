using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using Prism.Logging;
using Prism.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using StudyBuddy.Models;

namespace StudyBuddy.ViewModels
{
    public class GroupCreationPageViewModel : ViewModelBase
    {
        private INavigationService _navigationService;
        public event PropertyChangedEventHandler ChangedProperty;
        public System.Windows.Input.ICommand CreateGroupCommand { get; protected set; }
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

        public GroupCreationPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            CreateGroupCommand = new Command(CreateGroup);

            _navigationService = navigationService;
        }

        public async void CreateGroup()
        {
            Group group = new Group()
            {
                CreatorName = MainPageViewModel.CurrentGoogleUsername,
                GroupCode = "IFB101",
                GroupName = "Building Information Systems",
                Image = "Test"
            };

            await _navigationService.GoBackAsync();
        }

        private void OnChangedProperty([CallerMemberName] string propertyName = "")
        {
            var handler = ChangedProperty;
            if (handler != null)
                ChangedProperty(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
