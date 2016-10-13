using System;

using Android.App;
using Android.Content;
using System.Threading.Tasks;
using LocationChanger.Android.Services;
using Location.Droid.Services;

namespace LocationChanger.Services
{
    public class AppService
    {
        public event EventHandler<ServiceConnectedEventArgs> LocationServiceConnected = delegate { };
        protected static LocationServiceConnection locationServiceConnection;
        public static AppService Current
        {
            get { return current; }
        }
        private static AppService current;

        public LocationService LocationService
        {
            get
            {
                if (locationServiceConnection.Binder == null)
                    throw new Exception("Service not bound yet");
                return locationServiceConnection.Binder.Service;
            }
        }

        static AppService()
        {
            current = new AppService();
        }

        public AppService()
        {
            locationServiceConnection = new LocationServiceConnection(null);
            locationServiceConnection.ServiceConnected += (object sender, ServiceConnectedEventArgs e) => {
                LocationServiceConnected(this, e);
            };
        }

        public static void StartLocationService()
        {
            new Task(() => {
                Application.Context.StartService(new Intent(Application.Context, typeof(LocationService)));
                Intent locationServiceIntent = new Intent(Application.Context, typeof(LocationService));
                Application.Context.BindService(locationServiceIntent, locationServiceConnection, Bind.AutoCreate);
            }).Start();
        }

        public static void StopLocationService()
        {
            if (locationServiceConnection != null)
            {
                Application.Context.UnbindService(locationServiceConnection);
            }
            if (Current.LocationService != null)
            {
                Current.LocationService.StopSelf();
            }
        }
    }
}