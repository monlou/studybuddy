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
        public event PropertyChangedEventHandler PropertyChanged;
        public DelegateCommand SubmitButtonCommand { get; protected set; }

        private INavigationService _navigationService;

        private string _exclamation;
        private int _correctSubmissions;
        private CardDeck _deck;

        private bool _hasSubmitted;
        public bool HasSubmitted
        {
            get { return _hasSubmitted; }
            set
            {
                _hasSubmitted = value;
                OnPropertyChanged(nameof(HasSubmitted));
            }
        }

        private string _submission;
        public string Submission
        {
            get { return _submission; }
            set
            {
                _submission = value;
                OnPropertyChanged(nameof(Submission));
            }
        }

        private int _length;
        public int Length
        {
            get { return _length; }
            set
            {
                _length = value;
                OnPropertyChanged(nameof(Length));
            }
        }

        private int _counter;
        public int Counter
        {
            get { return _counter; }
            set
            {
                _counter = value;
                OnPropertyChanged(nameof(Counter));
            }
        }


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

            SubmitButtonCommand = new DelegateCommand(SubmitAnswer);

            _deck = new CardDeck();
            _correctSubmissions = 0;
            Counter = 1;
            _exclamation = "";

            ResetPage();

            // Using Prism's EventAggregator, this ViewModel can receive information from another
            // ViewModel publishes it, and then call a function loaded with said information.
            MainPageViewModel.events.GetEvent<QuizEvent>().Subscribe(PrepareQuiz);
        }

        private void ResetPage()
        {
            Submission = "";
            HasSubmitted = false;
            SubmitButtonColour = Color.LightGray;
            SubmitButtonText = "Submit";
            Input = "";
        }

        // The user first enters their submission. It is determined whether this submission matches 
        // the supplied answer. 
        private void QuizLogic()
        {
            if (Submission == AnswerText.ToString())
            {
                SubmitButtonColour = Color.Green;
                SubmitButtonText = "Correct! Tap to Proceed";
                _correctSubmissions++;
            }
            else
            {
                SubmitButtonColour = Color.Red;
                SubmitButtonText = "Incorrect! Tap to Proceed";
            }

            HasSubmitted = true;
        }

        // Once the user receives the verdict on their submission, they are to tap the button again to proceed.
        private void SubmitAnswer()
        {
            if (HasSubmitted)
            {
                Counter++;
                ProceedLogic();
            }
            else
            {
                Submission = Input;
                QuizLogic();
            }
        }

        // If the user has reached the end of the flashcard deck, a function that handles the ending of the quiz is called.
        // If there are yet questions remaining, the information on the quiz screen is reset, and the question/answer text is
        // replaced with that of the following flashcard in the deck.
        private void ProceedLogic()
        {
            if (Counter > Length)
            {
                FinishQuiz();
            }
            else
            {
                ResetPage();
                QuestionText = _deck.DeckContents[Counter-1].QuestionText.ToString();
                AnswerText = _deck.DeckContents[Counter-1].AnswerText.ToString();
            }
        }

        // The function that is called when a subscription to the EventAggregator is fulfilled. The received details
        // are read into the ViewModel and the quiz is prepared from the beginning of the flashcard deck.
        private void PrepareQuiz(CardDeck quiz)
        {
            _deck = quiz;
            QuizName = _deck.Name.ToString();
            Creator = _deck.CreatorName.ToString();
            Length = _deck.Length;
            QuestionText = _deck.DeckContents[0].QuestionText.ToString();
            AnswerText = _deck.DeckContents[0].AnswerText.ToString();
        }

        // When the user finishes the quiz, an automated message is generated detailing their efforts.
        private async void FinishQuiz()
        {
            DecideExclamation();

            Message message = new Message()
            {
                ObjType = "Msg",
                SenderName = "AnnouncerBot",
                SenderAvatar = new Uri("http://icons.iconarchive.com/icons/danleech/simple/1024/android-icon.png"),
                Text = MainPageViewModel.CurrentGoogleUsername 
                + " just got " + _correctSubmissions + " out of " + Length 
                + " flashcards right playing " + Creator + "'s " + QuizName 
                + " deck! " + _exclamation,
                Category = "Announcement",
                Timestamp = DateTime.Now.Ticks.ToString()
            };

            await ChatDBService.UploadMessage(message);
            await _navigationService.GoBackAsync();
        }

        // The announcement posted to the chatroom will differ depending on the user's score.
        private void DecideExclamation()
        {
            if (_correctSubmissions == 0)
            {
                _exclamation = "Ouch! Study up, buddy.";
            }
            else if (_correctSubmissions < (Length / 2) && (_correctSubmissions > 0))
            {
                _exclamation = "Try harder next time!";

            }
            else if (_correctSubmissions == (Length / 2))
            {
                _exclamation = "Not bad, getting there.";

            }
            else if ((_correctSubmissions > (Length / 2)) && (_correctSubmissions < Length))
            {
                _exclamation = "Almost got it! Next time.";

            }
            else
            {
                _exclamation = "Wow! Be proud. Now make some flashcards of your own.";
            }
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
#pragma warning restore CS0114 // Member hides inherited member; missing override keyword
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
