using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using Prism.Logging;
using Prism.Services;

namespace StudyBuddy.ViewModels
{
    public class MaterialsPageViewModel : ViewModelBase
    {
        private INavigationService _navigationService;
        public DelegateCommand AddNewCardCommand { get; set; }
        public MaterialsPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            AddNewCardCommand = new DelegateCommand(AddNewCard);

            _navigationService = navigationService;
        }

        public async void AddNewCard()
        {
            await _navigationService.NavigateAsync("FlashCardsPage");
        }
    }
}
