using System;

using Android.Content;
using Android.OS;
using LocationChanger.Android.Services;

namespace Location.Droid.Services
{
    public class LocationServiceConnection : Java.Lang.Object, IServiceConnection
    {
        public event EventHandler<ServiceConnectedEventArgs> ServiceConnected = delegate { };
        public LocationServiceBinder Binder { get; set; }
        protected LocationServiceBinder binder;

        public LocationServiceConnection(LocationServiceBinder binder)
        {
            if (binder != null)
            {
                this.binder = binder;
            }
        }

        public void OnServiceConnected(ComponentName name, IBinder service)
        {
            LocationServiceBinder serviceBinder = service as LocationServiceBinder;
            if (serviceBinder != null)
            {
                binder = serviceBinder;
                binder.IsBound = true;
                ServiceConnected(this, new ServiceConnectedEventArgs() { Binder = service });
                serviceBinder.Service.StartLocationUpdates();
            }
        }

        public void OnServiceDisconnected(ComponentName name)
        {
            binder.IsBound = false;
        }
    }
}

