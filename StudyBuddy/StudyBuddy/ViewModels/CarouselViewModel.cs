using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StudyBuddy.ViewModels
{
	public class CarouselViewModel : ViewModelBase
    {
        public DelegateCommand NavProfileCommand { get; set; }

        private INavigationService _navigationService;

        public CarouselViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;

            NavProfileCommand = new DelegateCommand(NavProfile);
        }

        private async void NavProfile()
        {
            await _navigationService.NavigateAsync("ProfilePage");
        }
    }
}
