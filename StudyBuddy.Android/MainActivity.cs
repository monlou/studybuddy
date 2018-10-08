using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Gms.Common.Apis;
using Android.Support.V7.App;
using Android.Gms.Common;
using Android.Util;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Auth.Api;
using System;
using System.Text;
using Android.Text.Method;

namespace StudyBuddy.Droid
{
    [Activity(Label = "StudyBuddy", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true)] //ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]

    [Register("com.ifb330.studybuddy.MainActivity")]

    public class MainActivity : AppCompatActivity, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener
    {
        //LaunchMode = LaunchMode.SingleTask; 

        private Button loginButton;
        private Button logoutButton;
        private TextView userDetailsTextView;
        GoogleApiClient mGoogleClient; 

        public int SIGN_IN_ID = 9001; 

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            FindViewById(Resource.Id.LoginButton).Visibility = ViewStates.Visible;
            FindViewById(Resource.Id.LogoutButton).Visibility = ViewStates.Gone;

            loginButton = FindViewById<Button>(Resource.Id.LoginButton);
            loginButton.Click += OnSignInClick;

            logoutButton = FindViewById<Button>(Resource.Id.LogoutButton);
            logoutButton.Click += OnSignOutClick;

            userDetailsTextView = FindViewById<TextView>(Resource.Id.UserDetailsTextView);
            userDetailsTextView.MovementMethod = new ScrollingMovementMethod();
            userDetailsTextView.Text = String.Empty;

            ConfigureGoogleSignIn(); 

            //global::Xamarin.Forms.Forms.Init(this, bundle);
            //LoadApplication(new App());

        }

        public void UpdateUI(bool isSignedIn)
        {
            if (isSignedIn)
            {
                FindViewById(Resource.Id.LoginButton).Visibility = ViewStates.Gone;
                FindViewById(Resource.Id.LogoutButton).Visibility = ViewStates.Visible;
            }
            else
            {
                FindViewById(Resource.Id.LoginButton).Visibility = ViewStates.Visible;
                FindViewById(Resource.Id.LogoutButton).Visibility = ViewStates.Gone;
            }
        }

        private void ConfigureGoogleSignIn()
        {
            GoogleSignInOptions google_options = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                .RequestEmail()
                .Build();

            mGoogleClient = new GoogleApiClient.Builder(this)
                .EnableAutoManage(this, this)
                .AddApi(Auth.GOOGLE_SIGN_IN_API, google_options)
                .AddConnectionCallbacks(this)
                .Build();
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if(requestCode == SIGN_IN_ID) {
                var result = Auth.GoogleSignInApi.GetSignInResultFromIntent(data);
                SignInResultHandler(result); 
            }
        }

        void OnSignInClick(object sender, EventArgs eventArgs)
        {
            var signInIntent = Auth.GoogleSignInApi.GetSignInIntent(mGoogleClient);
            StartActivityForResult(signInIntent, SIGN_IN_ID);
        }

        private void SignInResultHandler(GoogleSignInResult result)
        {
            if (result.IsSuccess)
            {
                var accountDetails = result.SignInAccount;
                UpdateUI(true); 
            }
        }

        void OnSignOutClick(object sender, EventArgs eventArgs)
        {
            UpdateUI(false); 
        }

        /*void OnSignout()
        {
            Auth.GoogleSignInApi.SignOut(GoogleClient).SetResultCallback(new SignOutResultCallback { Activity = this });
        }*/

        public void OnConnected(Bundle connectionHint)
        {
            //throw new NotImplementedException();
        }

        public void OnConnectionSuspended(int cause)
        {
            //throw new NotImplementedException();
        }

        public void OnConnectionFailed(ConnectionResult result)
        {
            //throw new NotImplementedException();
        }
    }
}
