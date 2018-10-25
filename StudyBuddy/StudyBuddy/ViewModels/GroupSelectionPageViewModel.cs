using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using Prism.Logging;
using Prism.Services;
using StudyBuddy.Models;
using System.ComponentModel;
using Xamarin.Forms;
using System.Runtime.CompilerServices;
using StudyBuddy.Services;

namespace StudyBuddy.ViewModels
{
    public class GroupSelectionPageViewModel : ViewModelBase
    {
        private INavigationService _navigationService;
        public event PropertyChangedEventHandler ChangedProperty;
        public System.Windows.Input.ICommand CreateGroupCommand { get; protected set; }
        public DelegateCommand LoadGroupsCommand { get; set; }
        public DelegateCommand SelectGroupCommand { get; set; }
        public DelegateCommand SearchGroupsCommand { get; set; }

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

        public GroupSelectionPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            LoadGroupsCommand = new DelegateCommand(LoadGroups);
            SelectGroupCommand = new DelegateCommand(SelectGroup);
            CreateGroupCommand = new DelegateCommand(CreateGroup);
            SearchGroupsCommand = new DelegateCommand(SearchGroups);

            _navigationService = navigationService;
        }

        public void LoadGroups()
        {
            /*Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Hello ContentPage" }
                }
            };*/
        }

        public async void SelectGroup()
        {
            await _navigationService.NavigateAsync("Carousel");
        }

        public async void CreateGroup()
        {
            await _navigationService.NavigateAsync("Carousel");
        }

        public void SearchGroups() {

        }

        private void OnChangedProperty([CallerMemberName] string propertyName = "")
        {
            var handler = ChangedProperty;
            if (handler != null)
                ChangedProperty(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
