using Android.App;
using Android.Widget;
using Android.OS;
using Android.Locations;
using LocationChanger.Android.Services;

namespace LocationChanger.Android
{
    [Activity(Label = "LocationChanger.Android", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        Button _button;
        // labels for getting location results
        TextView _latText;
        TextView _longText;
        int _count = 1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            _button = FindViewById<Button>(Resource.Id.MyButton);
            _latText = FindViewById<TextView>(Resource.Id.lat);
            _longText = FindViewById<TextView>(Resource.Id.longx);

            _button.Click += delegate
            {
                _button.Text = $"{_count++} clicks!";
            };
            AppService.StartLocationService();
        }

        protected override void OnDestroy()
        {
            AppService.StopLocationService();
            base.OnDestroy();
        }

        public void HandleLocationChanged(object sender, LocationChangedEventArgs e)
        {
            Location location = e.Location;
            RunOnUiThread(() => {
                _latText.Text = $"Latitude: {location.Latitude}";
                _longText.Text = $"Longitude: {location.Longitude}";
            });
        }
    }
}

