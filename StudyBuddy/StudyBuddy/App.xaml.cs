using Prism;
using Prism.Ioc;
using StudyBuddy.ViewModels;
using StudyBuddy.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Prism.Unity;
using Prism.Events;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;


[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace StudyBuddy
{
    public partial class App : PrismApplication
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<Carousel>();

            containerRegistry.RegisterForNavigation<MainPage>();
            containerRegistry.RegisterForNavigation<ProfilePage>();

            containerRegistry.RegisterForNavigation<GroupSelectionPage>();
            containerRegistry.RegisterForNavigation<GroupCreationPage>();

            containerRegistry.RegisterForNavigation<ChatPage>();

            containerRegistry.RegisterForNavigation<FlashCardsPage>();
            containerRegistry.RegisterForNavigation<MaterialsPage>();
            containerRegistry.RegisterForNavigation<FlashCardQuizPage>();

            containerRegistry.RegisterSingleton<IEventAggregator, EventAggregator>();
        }
    }
}
