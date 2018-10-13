using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using StudyBuddy.Services.Contracts;
using Prism.Services;
using StudyBuddy.Models;


namespace StudyBuddy.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private INavigationService _navigationService;
        private readonly IGoogleManager _googleManager;
        private readonly IPageDialogService _dialogService;

        public DelegateCommand LoginCommand { get; set; }

        private GoogleUser _googleUser;

        public GoogleUser GoogleUser
        {
            get { return _googleUser; }
            set { SetProperty(ref _googleUser, value); }
        }

        public MainPageViewModel(INavigationService navigationService, IGoogleManager googleManager, IPageDialogService dialogService)
            : base(navigationService)
        {
            Title = "Main Page";
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
                GoogleUser = googleUser;
                _navigationService.NavigateAsync("Carousel");
            }
            else
            {
                _dialogService.DisplayAlertAsync("Error", message, "Ok");
            }
        }

    }
}
