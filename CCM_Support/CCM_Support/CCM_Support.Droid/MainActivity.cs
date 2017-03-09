using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Gms.Common;
using Android.Util;
using Android.Content;
using PushNotification.Plugin;
//using HockeyApp.Android;
//using HockeyApp.Android.Metrics;

namespace CCM_Support.Droid
{
    [Activity(Label = "CCM_Support", Icon = "@drawable/Sybrin_Stacked", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            //CrashManager.Register(this, "0647a7cb6e174f408643bc4c1f145c81");
            //MetricsManager.Register(Application, "0647a7cb6e174f408643bc4c1f145c81");

            CheckForUpdates();

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
        }

        private void CheckForUpdates()
        {
            // Remove this for store builds!
            //UpdateManager.Register(this, "0647a7cb6e174f408643bc4c1f145c81");
        }

        private void UnregisterManagers()
        {
            //UpdateManager.Unregister();
        }

        protected override void OnPause()
        {
            base.OnPause();
            UnregisterManagers();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            UnregisterManagers();
        }
    }
}

