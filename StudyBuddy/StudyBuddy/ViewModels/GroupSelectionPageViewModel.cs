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
using System.Collections.ObjectModel;

namespace StudyBuddy.ViewModels
{
    public class GroupSelectionPageViewModel : ViewModelBase
    {
        private INavigationService _navigationService;
        static ChatDBService chat;
        static FlashDBService flash;

        public DelegateCommand NavProfileCommand { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        public System.Windows.Input.ICommand CreateGroupCommand { get; protected set; }

        private List<Group> _loadedGroups;
        public ObservableCollection<Group> LoadedGroups { get; } = new ObservableCollection<Group>();

        public DelegateCommand LoadGroupsCommand { get; set; }
        public DelegateCommand SelectGroupCommand { get; set; }
        public DelegateCommand SearchGroupsCommand { get; set; }

        public static string SelectedDBName;

        private Group _selectedGroup;
        public Group SelectedGroup
        {
            get { return _selectedGroup; }
            set
            {
                _selectedGroup = value;
                OnPropertyChanged(nameof(SelectedGroup));
            }
        }

        private string _input;
        public string Input
        {
            get { return _input; }
            set
            {
                _input = value;
                OnPropertyChanged(nameof(Input));
            }
        }

        public GroupSelectionPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            SelectGroupCommand = new DelegateCommand(SelectGroup);
            CreateGroupCommand = new DelegateCommand(CreateGroup);
            NavProfileCommand = new DelegateCommand(NavProfile);


            _loadedGroups = new List<Group>();
            LoadGroups();


            _navigationService = navigationService;
        }


        private async void SelectGroup()
        {
            Console.WriteLine("Hit Selected Group");

            if (SelectedGroup == null)
            {
                return;
            } 

            SelectedDBName = SelectedGroup.GroupSubjectCode.ToString();
            Console.WriteLine("Selected group is: " + SelectedDBName);
            InitializeServices();
            await _navigationService.NavigateAsync("Carousel");
        }

        private async void InitializeServices()
        {
            chat = new ChatDBService();
            flash = new FlashDBService();
            await chat.RunChangeFeedHostAsync();
            await flash.RunChangeFeedHostAsync();
        }


        private async void CreateGroup()
        {
            await _navigationService.NavigateAsync("GroupCreationPage");
        }

        private async void LoadGroups() {

            _loadedGroups = await GroupDBService.LoadGroups();

            foreach (var group in _loadedGroups)
            {
                this.LoadedGroups.Add(group);
            }
        }

        public async void NavProfile()
        {
            await _navigationService.NavigateAsync("ProfilePage");
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var handler = PropertyChanged;

            if (handler != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
