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
        public System.Windows.Input.ICommand SubmitCommand { get; protected set; }

        //public static CardDeck loadedQuiz = new CardDeck();

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

        private string _wrong1Text;
        public string Wrong1Text
        {
            get { return _wrong1Text; }
            set
            {
                _wrong1Text = value;
                OnPropertyChanged(nameof(Wrong1Text));
            }
        }

        private string _wrong2Text;
        public string Wrong2Text
        {
            get { return _wrong2Text; }
            set
            {
                _wrong2Text = value;
                OnPropertyChanged(nameof(Wrong2Text));
            }
        }

        public FlashCardQuizPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            SubmitCommand = new Command(SubmitAnswer);

            MainPageViewModel.events.GetEvent<QuizEvent>().Subscribe(PrepareQuiz);
        }

        private void SubmitAnswer()
        {
            Console.WriteLine("HIT SUBMIT ANSWER");

            Console.WriteLine(QuizName);

            //Console.WriteLine("loaded quiz name: " + loadedQuiz.Name.ToString());



            //QuizName = loadedQuiz.Name.ToString();
            //Creator = loadedQuiz.CreatorName.ToString();
            //QuestionText = loadedQuiz.DeckContents[0].QuestionText.ToString();
            //AnswerText = loadedQuiz.DeckContents[0].CorrectText.ToString();
            //Wrong1Text = loadedQuiz.DeckContents[0].WrongTextOne.ToString();
            //Wrong2Text = loadedQuiz.DeckContents[0].WrongTextTwo.ToString();

            Console.WriteLine(AnswerText);
        }

        public void PrepareQuiz(CardDeck quiz)
        {
            Console.WriteLine("HIT PREPARE QUIZ");


            //loadedQuiz = quiz;

            QuizName = quiz.Name.ToString();
            Creator = quiz.CreatorName.ToString();
            QuestionText = quiz.DeckContents[0].QuestionText.ToString();
            AnswerText = quiz.DeckContents[0].CorrectText.ToString();
            Wrong1Text = quiz.DeckContents[0].WrongTextOne.ToString();
            Wrong2Text = quiz.DeckContents[0].WrongTextTwo.ToString();

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
