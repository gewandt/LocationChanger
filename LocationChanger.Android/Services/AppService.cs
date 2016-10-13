using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;

namespace LocationChanger.Android.Services
{
    public class AppService
    {
        public event EventHandler<ServiceConnectedEventArgs> LocationServiceConnected = delegate { };
        protected static LocationServiceConnection LocationServiceConnection;
        public static AppService Current { get; }

        public LocationService LocationService
        {
            get
            {
                if (LocationServiceConnection.Binder == null)
                    throw new Exception("Service not bound yet");
                return LocationServiceConnection.Binder.Service;
            }
        }

        static AppService()
        {
            Current = new AppService();
        }

        public AppService()
        {
            LocationServiceConnection = new LocationServiceConnection(null);
            LocationServiceConnection.ServiceConnected += (object sender, ServiceConnectedEventArgs e) => {
                LocationServiceConnected(this, e);
            };
        }

        public static void StartLocationService()
        {
            new Task(() => {
                Application.Context.StartService(new Intent(Application.Context, typeof(LocationService)));
                Intent locationServiceIntent = new Intent(Application.Context, typeof(LocationService));
                Application.Context.BindService(locationServiceIntent, LocationServiceConnection, Bind.AutoCreate);
            }).Start();
        }

        public static void StopLocationService()
        {
            if (LocationServiceConnection != null)
            {
                Application.Context.UnbindService(LocationServiceConnection);
            }
            Current.LocationService?.StopSelf();
        }
    }
}