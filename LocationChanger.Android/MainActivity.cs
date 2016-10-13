using System;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Locations;
using LocationChanger.Services;

namespace LocationChanger.Android
{
    [Activity(Label = "LocationChanger.Android", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        Button button;
        // labels for getting location results
        TextView latText;
        TextView longText;
        int count = 1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            button = FindViewById<Button>(Resource.Id.MyButton);
            latText = FindViewById<TextView>(Resource.Id.lat);
            longText = FindViewById<TextView>(Resource.Id.longx);

            button.Click += delegate { button.Text = string.Format("{0} clicks!", count++); };
            AppService.StartLocationService();
        }

        protected override void OnPause()
        {
            base.OnPause();
        }

        protected override void OnResume()
        {
            base.OnResume();
        }

        protected override void OnDestroy()
        {
            AppService.StopLocationService();
            base.OnDestroy();
        }

        public void HandleLocationChanged(object sender, LocationChangedEventArgs e)
        {
            global::Android.Locations.Location location = e.Location;
            RunOnUiThread(() => {
                latText.Text = $"Latitude: {location.Latitude}";
                longText.Text = $"Longitude: {location.Longitude}";
            });
        }
    }
}

