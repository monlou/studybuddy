using System;
using Android.Gms.Common.Apis;
using Java.Lang;

namespace StudyBuddy.Droid.Auth
{
    public class SignOutResultCallback : Java.Lang.Object, IResultCallback
    {
        public MainActivity Activity { get; set; }

        public void OnResult(Java.Lang.Object result)
        {
            Activity.UpdateUI(false);
        }
    }
}
