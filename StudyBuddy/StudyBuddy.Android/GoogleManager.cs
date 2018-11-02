using System;
using Android.Content;
using Android.Gms.Auth.Api;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.OS;
using StudyBuddy.Models;
using StudyBuddy.Services.Contracts;

namespace StudyBuddy.Droid
{
    public class GoogleManager : Java.Lang.Object, IGoogleManager, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener
    {
        public Action<GoogleUser, string> _onLoginComplete;
        public static GoogleApiClient _googleApiClient { get; set; }
        public static GoogleManager Instance { get; private set; }

        public GoogleManager()
        {
            Instance = this;
            GoogleSignInOptions signInOptions = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                                                             .RequestEmail()
                                                             .Build();

            _googleApiClient = new GoogleApiClient.Builder((MainActivity.Instance).ApplicationContext)
                .AddConnectionCallbacks(this)
                .AddOnConnectionFailedListener(this)
                .AddApi(Auth.GOOGLE_SIGN_IN_API, signInOptions)
                .AddScope(new Scope(Scopes.Profile))
                .Build();
        }

        // The dependency injection is invoked and a new Google User
        // is outfitted with the response data from authentication.
        public void OnAuthCompleted(GoogleSignInResult result)
        {
            if (result.IsSuccess)
            {
                GoogleSignInAccount account = result.SignInAccount;
                _onLoginComplete?.Invoke(new GoogleUser()
                {
                    Name = account.DisplayName,
                    Email = account.Email,
                    Picture = new Uri($"{account.PhotoUrl}")
                }, string.Empty);
            }
            else
            {
                Console.WriteLine(result.Status);
                _onLoginComplete?.Invoke(null, "An error has occurred!");
            }
        }

        public void Login(Action<GoogleUser, string> onLoginComplete)
        {
            _onLoginComplete = onLoginComplete;
            Intent signInIntent = Auth.GoogleSignInApi.GetSignInIntent(_googleApiClient);
            (MainActivity.Instance).StartActivityForResult(signInIntent, 1);
            _googleApiClient.Connect();
        }

        public void Logout()
        {
            _googleApiClient.Disconnect();
        }

        public void OnConnected(Bundle connectionHint)
        {

        }

        public void OnConnectionSuspended(int cause)
        {
            _onLoginComplete?.Invoke(null, "Cancelled!");
        }

        public void OnConnectionFailed(ConnectionResult result)
        {
            _onLoginComplete?.Invoke(null, result.ErrorMessage);
        }
    }
}