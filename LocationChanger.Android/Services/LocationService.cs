using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Locations;

namespace LocationChanger.Android.Services
{
    [Service]
    public class LocationService : Service, ILocationListener
    {
        private event EventHandler<LocationChangedEventArgs> LocationChanged = delegate { };
        private event EventHandler<ProviderDisabledEventArgs> ProviderDisabled = delegate { };
        private event EventHandler<ProviderEnabledEventArgs> ProviderEnabled = delegate { };
        private event EventHandler<StatusChangedEventArgs> StatusChanged = delegate { };
        protected LocationManager LocMgr = Application.Context.GetSystemService("location") as LocationManager;
        private IBinder _binder;
        
        public override IBinder OnBind(Intent intent)
        {
            _binder = new LocationServiceBinder(this);
            return _binder;
        }

        public void OnProviderDisabled(string provider)
        {
            ProviderDisabled(this, new ProviderDisabledEventArgs(provider));
        }

        public void OnProviderEnabled(string provider)
        {
            ProviderEnabled(this, new ProviderEnabledEventArgs(provider));
        }

        public void OnStatusChanged(string provider, Availability status, Bundle extras)
        {
            StatusChanged(this, new StatusChangedEventArgs(provider, status, extras));
        }

        public void OnLocationChanged(global::Android.Locations.Location location)
        {
            LocationChanged(this, new LocationChangedEventArgs(location));
        }

        public void StartLocationUpdates()
        {
            var locationCriteria = new Criteria();

            locationCriteria.Accuracy = Accuracy.NoRequirement;
            locationCriteria.PowerRequirement = Power.NoRequirement;

            var locationProvider = LocMgr.GetBestProvider(locationCriteria, true);

            LocMgr.RequestLocationUpdates(locationProvider, 2000, 0, this);
        }
    }
}