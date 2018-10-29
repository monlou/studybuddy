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
        private INavigationService _navigationService;
        public event PropertyChangedEventHandler PropertyChanged;
        public System.Windows.Input.ICommand CreateGroupCommand { get; protected set; }

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
            CreateGroupCommand = new Command(CreateGroup);

            _navigationService = navigationService;
        }

        public async void CreateGroup()
        {
            Group group = new Group()
            {
                GroupOwner = MainPageViewModel.CurrentGoogleUsername,
                GroupSubjectCode = SubjectCode,
                GroupSubjectName = SubjectName
            };

            Console.WriteLine(group.ToString());

            await GroupDBService.UploadGroup(group);

            await _navigationService.GoBackAsync();
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var handler = PropertyChanged;

            if (handler != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
