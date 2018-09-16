using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Text.Method;
using Android.Widget;
using Auth0.OidcClient;
using IdentityModel.OidcClient;
using System;
using System.Text;

namespace StudyBuddy.Droid
{
    [Activity(Label = "StudyBuddy", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]

    [IntentFilter(
    new[] { Intent.ActionView },
    Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataScheme = Configuration.Scheme,
        DataHost = Configuration.Host,
        DataPathPrefix = Configuration.Path)]

    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        //LaunchMode = LaunchMode.SingleTask; 

        private Button loginButton;
        private TextView userDetailsTextView;
        private Auth0Client client;
        private AuthorizeState authorizeState; 

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            loginButton = FindViewById<Button>(Resource.Id.LoginButton);
            loginButton.Click += LoginButtonOnClick;

            userDetailsTextView = FindViewById<TextView>(Resource.Id.UserDetailsTextView);
            userDetailsTextView.MovementMethod = new ScrollingMovementMethod();
            userDetailsTextView.Text = String.Empty;

            //global::Xamarin.Forms.Forms.Init(this, bundle);
            //LoadApplication(new App());

            client = new Auth0Client(new Auth0ClientOptions
            {
                Domain = Configuration.Domain,
                ClientId = Configuration.Client
                //Scope = Configuration.Scope
                //Activity = this
            });
        }

        protected override void OnResume()
        {
            base.OnResume();
        }

        private async void LoginButtonOnClick(object sender, EventArgs eventArgs)
        {
            userDetailsTextView.Text = "";

            // Prepare for the login
            authorizeState = await client.PrepareLoginAsync();

            // Send the user off to the authorization endpoint
            var uri = Android.Net.Uri.Parse(authorizeState.StartUrl);
            var intent = new Intent(Intent.ActionView, uri);
            intent.AddFlags(ActivityFlags.NoHistory);
            StartActivity(intent);
        }

        protected override async void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);

            var loginResult = await client.ProcessResponseAsync(intent.DataString, authorizeState);

            var sb = new StringBuilder();
            if (loginResult.IsError)
            {
                sb.AppendLine($"An error occurred during login: {loginResult.Error}");
            }
            else
            {
                sb.AppendLine($"ID Token: {loginResult.IdentityToken}");
                sb.AppendLine($"Access Token: {loginResult.AccessToken}");
                sb.AppendLine($"Refresh Token: {loginResult.RefreshToken}");

                sb.AppendLine();

                sb.AppendLine("-- Claims --");
                foreach (var claim in loginResult.User.Claims)
                {
                    sb.AppendLine($"{claim.Type} = {claim.Value}");
                }
            }

            userDetailsTextView.Text = sb.ToString();
        }
    }
}
