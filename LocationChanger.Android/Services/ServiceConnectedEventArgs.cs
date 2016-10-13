using System;
using Android.OS;

namespace LocationChanger.Android.Services
{
    public class ServiceConnectedEventArgs : EventArgs
    {
        public IBinder Binder { get; set; }
    }
}