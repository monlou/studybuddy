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
using Xamarin.Forms;
using StudyBuddy.Models;
using StudyBuddy.Services;

namespace StudyBuddy.ViewModels
{
    public class GroupCreationPageViewModel : ViewModelBase
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public DelegateCommand CreateGroupCommand { get; protected set; }

        private INavigationService _navigationService;

        private string _subjectCode;
        public string SubjectCode
        {
            get { return _subjectCode; }
            set
            {
                _subjectCode = value;
                OnPropertyChanged(nameof(SubjectCode));
            }
        }

        private string _subjectName;
        public string SubjectName
        {
            get { return _subjectName; }
            set
            {
                _subjectName = value;
                OnPropertyChanged(nameof(SubjectName));
            }
        }

        public GroupCreationPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;

            CreateGroupCommand = new DelegateCommand(CreateGroup);
        }

        private async void CreateGroup()
        {
            // The user must both assign a name and a code or the group will not be created and published to the database.
            if (SubjectCode == null || SubjectName == null)
            {
                return;
            }

            Group group = new Group()
            {
                GroupOwner = MainPageViewModel.CurrentGoogleUsername,
                GroupSubjectCode = SubjectCode,
                GroupSubjectName = SubjectName
            };

            Console.WriteLine(group.ToString());

            await GroupDBService.UploadGroup(group);
            InitializeDB();

            await _navigationService.NavigateAsync("GroupSelectionPage");
        }

        // Using the assigned code, a new database is created in the Azure Cosmos DB with the appropriate collections.
        private async void InitializeDB()
        {
            await GroupDBService.CreateDatabase(SubjectCode);
            await GroupDBService.CreateDocumentCollection(SubjectCode, "Messages");
            await GroupDBService.CreateDocumentCollection(SubjectCode, "Flashcards");
            await GroupDBService.CreateDocumentCollection(SubjectCode, "Lease");
            await GroupDBService.CreateDocumentCollection(SubjectCode, "FlashLease");
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
