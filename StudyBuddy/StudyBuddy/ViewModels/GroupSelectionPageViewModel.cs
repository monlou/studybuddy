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
        public event PropertyChangedEventHandler PropertyChanged;
        public DelegateCommand LoadGroupsCommand { get; set; }
        public DelegateCommand SelectGroupCommand { get; set; }
        public DelegateCommand NavProfileCommand { get; set; }
        public DelegateCommand CreateGroupCommand { get; protected set; }

        // Only one version of the chat and flash services are required per client.
        static ChatDBService chat;
        static FlashDBService flash;

        public ObservableCollection<Group> LoadedGroups { get; } = new ObservableCollection<Group>();
        public static string SelectedDBName;

        private INavigationService _navigationService;
        private List<Group> _loadedGroups;

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
            _navigationService = navigationService;

            SelectGroupCommand = new DelegateCommand(SelectGroup);
            CreateGroupCommand = new DelegateCommand(CreateGroup);
            NavProfileCommand = new DelegateCommand(NavProfile);

            _loadedGroups = new List<Group>();
            LoadGroups();
        }


        private async void SelectGroup()
        {
            // If the user has not selected a group, they cannot proceed.
            if (SelectedGroup == null)
            {
                return;
            } 

            SelectedDBName = SelectedGroup.GroupSubjectCode.ToString();
            InitializeServices();
            await _navigationService.NavigateAsync("Carousel");
        }

        // The DB services are initialized with the selected group's information, tying them to the correct locations
        // in Cosmos DB. 
        private async void InitializeServices()
        {
            chat = new ChatDBService();
            flash = new FlashDBService();

            // Initializes the ChangeFeedProcessor.
            await chat.RunChangeFeedHostAsync();
            await flash.RunChangeFeedHostAsync();
        }


        private async void CreateGroup()
        {
            await _navigationService.NavigateAsync("GroupCreationPage");
        }


        // When the page is constructed, the client receives all pre-existing groups
        // from the database. These groups are loaded into a temporary list which are
        // then read into the ObservableCollection. This intermediary step was made 
        // necessary due to the fact that ObservableCollections cannot be assigned to Lists.
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

        // Boilerplate responsible for acknowledging changes between the two-way View/ViewModel binding.
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var handler = PropertyChanged;

            if (handler != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
