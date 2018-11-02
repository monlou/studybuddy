using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Auth.Api;
using Prism;
using Prism.Ioc;
using Xamarin.Forms;
using StudyBuddy.Services.Contracts;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace StudyBuddy.Droid
{
    [Activity(Label = "StudyBuddy", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {

        internal static MainActivity Instance { get; private set; }

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar; 

            base.OnCreate(bundle);
            Instance = this;

            Forms.Init(this, bundle);
            DependencyService.Register<IGoogleManager, GoogleManager>();

            // Register with the App Center for testing purposes.
            AppCenter.Start("18fc2ed5-04a0-4594-a87e-0fd1fa8ebae2",
                   typeof(Analytics), typeof(Crashes));

            LoadApplication(new App(new AndroidInitializer()));
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Android.Content.Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == 1)
            {
                GoogleSignInResult result = Auth.GoogleSignInApi.GetSignInResultFromIntent(data);
                GoogleManager.Instance.OnAuthCompleted(result);
            }
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry container)
        {
            container.Register<IGoogleManager, GoogleManager>();
        }
    }
}

