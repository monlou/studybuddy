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

        public DelegateCommand GoogleLoginCommand { get; set; }
        public DelegateCommand GoogleLogoutCommand { get; set; }

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
            GoogleLoginCommand = new DelegateCommand(Login);
            _googleManager = googleManager;
            _dialogService = dialogService;

            //GoogleLoginCommand = new DelegateCommand(NavigateToChatPageCall);
        }




        public void Login()
        {
            Console.WriteLine("==============Hit NavigateToChatPageCall");
            _googleManager.Login(OnLoginComplete);
        }

        private void OnLoginComplete(GoogleUser googleUser, string message)
        {
            if (googleUser != null)
            {
                GoogleUser = googleUser;
                _navigationService.NavigateAsync("ChatPage");
            }
            else
            {
                _dialogService.DisplayAlertAsync("Error", message, "Ok");
            }
        }

    }
}
