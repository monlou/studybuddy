using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using Prism.Logging;
using Prism.Services;
using Prism.Mvvm;
using StudyBuddy.Services.Contracts;
using StudyBuddy.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using StudyBuddy.Services;
using Prism.Events;

#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
namespace StudyBuddy.ViewModels
{
    public class FlashCardQuizPageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private INavigationService _navigationService;

        public event PropertyChangedEventHandler PropertyChanged;
        public System.Windows.Input.ICommand SubmitButtonCommand { get; protected set; }

        private string _submission;

        private string _quizName;
        public string QuizName
        {
            get { return _quizName; }
            set
            {
                _quizName = value;
                OnPropertyChanged(nameof(QuizName));
            }
        }

        private string _creator;
        public string Creator
        {
            get { return _creator; }
            set
            {
                _creator = value;
                OnPropertyChanged(nameof(Creator));
            }
        }

        private string _questionText;
        public string QuestionText
        {
            get { return _questionText; }
            set
            {
                _questionText = value;
                OnPropertyChanged(nameof(QuestionText));
            }
        }

        private string _answerText;
        public string AnswerText
        {
            get { return _answerText; }
            set
            {
                _answerText = value;
                OnPropertyChanged(nameof(AnswerText));
            }
        }

        private string _submitButtonText;
        public string SubmitButtonText
        {
            get { return _submitButtonText; }
            set
            {
                _submitButtonText = value;
                OnPropertyChanged(nameof(SubmitButtonText));
            }
        }

        private Color _submitButtonColour;
        public Color SubmitButtonColour
        {
            get { return _submitButtonColour; }
            set
            {
                _submitButtonColour = value;
                OnPropertyChanged(nameof(SubmitButtonColour));
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



        public FlashCardQuizPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            SubmitButtonCommand = new Command(SubmitAnswer);
            SubmitButtonColour = Color.White;
            SubmitButtonText = "Submit";
            _submission = "";
            MainPageViewModel.events.GetEvent<QuizEvent>().Subscribe(PrepareQuiz);
        }

        private void SubmitAnswer()
        {
            Console.WriteLine("HIT SUBMIT ANSWER");
            _submission = Input;
            QuizLogic();
        }

        private void QuizLogic()
        {
            Console.WriteLine("HIT Quiz logic");
            if (_submission == AnswerText.ToString())
            {
                SubmitButtonColour = Color.Green;
                SubmitButtonText = "Correct!";
            }
            else
            {
                SubmitButtonColour = Color.Red;
                SubmitButtonText = "Incorrect!";

            }

        }

        public void PrepareQuiz(CardDeck quiz)
        {
            QuizName = quiz.Name.ToString();
            Creator = quiz.CreatorName.ToString();
            QuestionText = quiz.DeckContents[0].QuestionText.ToString();
            AnswerText = quiz.DeckContents[0].AnswerText.ToString();
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var handler = PropertyChanged;
            if (handler != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
#pragma warning restore CS0114 // Member hides inherited member; missing override keyword
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
