﻿using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using StudyBuddy.Services.Contracts;
using Prism.Services;
using StudyBuddy.Models;
using StudyBuddy.Services;
using System.Threading.Tasks;
using Prism.Events;

namespace StudyBuddy.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private INavigationService _navigationService;
        private readonly IGoogleManager _googleManager;
        private readonly IPageDialogService _dialogService;

        readonly GroupDBService master = new GroupDBService();

        public static EventAggregator events = new EventAggregator();

        public DelegateCommand LoginCommand { get; set; }

        private GoogleUser _googleUser;

        public GoogleUser CurrentGoogleUser
        {
            get { return _googleUser; }
            set { SetProperty(ref _googleUser, value); }
        }

        public static string CurrentGoogleUsername;
        public static string CurrentGoogleEmail;
        public static Uri CurrentGoogleAvatar;
        
        public MainPageViewModel(INavigationService navigationService, IGoogleManager googleManager, IPageDialogService dialogService)
            : base(navigationService)
        {
            _navigationService = navigationService;
            _googleManager = googleManager;
            _dialogService = dialogService;

            LoginCommand = new DelegateCommand(CallLogin);
        }

        public void CallLogin()
        {
            _googleManager.Login(OnLoginComplete);
        }

        private void OnLoginComplete(GoogleUser googleUser, string message)
        {
            if (googleUser != null)
            {
                CurrentGoogleUser = googleUser;
                CurrentGoogleUsername = CurrentGoogleUser.Name;
                CurrentGoogleAvatar = CurrentGoogleUser.Picture;
                CurrentGoogleEmail = CurrentGoogleUser.Email;
                NavigateToGroupPage();
            }
            else
            {
                _dialogService.DisplayAlertAsync("Login Error", message, "Ok");
            }
        }

        private async void NavigateToGroupPage()
        {
            await _navigationService.NavigateAsync("GroupSelectionPage");
        }

    }
}
