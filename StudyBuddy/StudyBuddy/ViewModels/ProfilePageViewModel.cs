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
using StudyBuddy.Services.Contracts;

namespace StudyBuddy.ViewModels
{
    public class ProfilePageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private INavigationService _navigationService;
        private readonly IGoogleManager _googleManager;

        public DelegateCommand LogoutCommand { get; set; }

        private Uri _avatar;
        public Uri Avatar
        {
            get { return _avatar; }
            set
            {
                _avatar = value;
                OnPropertyChanged(nameof(Avatar));
            }
        }

        private string _username;
        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public ProfilePageViewModel(IGoogleManager googleManager, INavigationService navigationService) : base(navigationService)
        {
            _googleManager = googleManager;
            _navigationService = navigationService;

            Avatar = MainPageViewModel.CurrentGoogleAvatar;
            Email = MainPageViewModel.CurrentGoogleEmail;
            Username = MainPageViewModel.CurrentGoogleUsername;

            LogoutCommand = new DelegateCommand(CallLogout);

        }

        private async void CallLogout()
        {
            _googleManager.Logout();
            await _navigationService.NavigateAsync("MainPage");
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var handler = PropertyChanged;

            if (handler != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
