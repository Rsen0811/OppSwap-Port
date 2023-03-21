using System;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics;
namespace OppSwap.ViewModels
{
	public partial class MainPageViewModel: ObservableObject
	{
        [ObservableProperty]
        String latitudeLongitude;
        [ObservableProperty]
        String timeTaken;
        public MainPageViewModel()
		{
            latitudeLongitude = "0 , 0";
            timeTaken = "0";
            Compass.Default.ReadingChanged += Compass_ReadingChanged;
            Compass.Default.Start(SensorSpeed.Default);

        }

        String currHeading;

        LatLong location;
        LatLong pole = new LatLong();
        private void Compass_ReadingChanged(object sender, CompassChangedEventArgs e)
        {
            // Update UI Label with compass state
            currHeading= $"Compass: {e.Reading}";
        }

        //only works with IOS as of right now
        [RelayCommand]
        public async Task GetCurrentLocation()
        {
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(30));
                Location l = await Geolocation.Default.GetLocationAsync(request);
                TimeTaken = stopwatch.ElapsedMilliseconds+"";
                
                stopwatch.Stop();
                location = new LatLong(l.Latitude,l.Longitude);
                //in the future keep in mind to create a check mock provider if to do something if it is a faked location
                LatitudeLongitude = l.Latitude+" , "+l.Longitude;
                TimeTaken = location.bearing(pole)+"";
            }
            // Catch one of the following exceptions:
            //   FeatureNotSupportedException
            //   FeatureNotEnabledException
            //   PermissionException
            catch (Exception ex)
            {
                throw(ex);
                // Unable to get location
            }
        }
    }
}

