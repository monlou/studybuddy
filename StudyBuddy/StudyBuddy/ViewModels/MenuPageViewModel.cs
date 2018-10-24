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
    public class MenuPageViewModel : ViewModelBase
    {
        public MenuPageViewModel(INavigationService navigationService) : base(navigationService)
        {
        }
    }
}
