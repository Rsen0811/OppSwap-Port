using System;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics;
using System.Numerics;
namespace OppSwap.ViewModels
{
    //[QueryProperty("RoomID"/*The name of the property in ViewModel*/, "RoomID"/*The name of the property when you switch pages*/)]

    public partial class RoomDetailPageViewModel: ObservableObject
	{

        [ObservableProperty]
        Room room;

		[ObservableProperty]
		String roomID;
        
		[ObservableProperty]
		String players;

		[ObservableProperty]
		String name;

		[ObservableProperty]
		String targetName;

        [ObservableProperty]
        bool targetAlive;

        [ObservableProperty]
        String targetId;

		[ObservableProperty]
		String targetPos;

        [ObservableProperty]
        String timeTaken;

        [ObservableProperty]
        String latitudeLongitude;

        [ObservableProperty]
        String currHeading;

        [ObservableProperty]
        double arrowAngle;

        LatLong location;
        LatLong pole = new LatLong();

        LatLong pos;

        public RoomDetailPageViewModel()
		{
            Room = new Room("bruh", "bruh");

            RoomID = Room.Id;
            Players = "";// Room.players.ToString();
            Name = Room.Name;
            TargetName = ""; //Room.target.Name;
            TargetId = "";//Room.target.Id;
			TargetAlive= false;//Room.target.IsAlive;
            pos = pole;//Room.target.position;
            TargetPos = pos.ToString();
            latitudeLongitude = "0 , 0";
            TimeTaken = "0";
            CurrHeading = "90";
            //Compass.Default.ReadingChanged += Compass_ReadingChanged;
            //Compass.Default.Start(SensorSpeed.Default);
            ArrowAngle = 0;
            
        }
        //only works with IOS as of right now
        [RelayCommand]
        public async Task getCurrentLocation()
        {   
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(30));
                Location l = await Geolocation.Default.GetLocationAsync(request);
                TimeTaken = stopwatch.ElapsedMilliseconds + "";

                stopwatch.Stop();
                location = new LatLong(l.Latitude, l.Longitude);
                //in the future keep in mind to create a check mock provider if to do something if it is a faked location
                LatitudeLongitude = l.Latitude + " , " + l.Longitude;
                
                getArrowAngle(location.bearing(pos), double.Parse(CurrHeading));
                TimeTaken = ArrowAngle + "";
            }
            // Catch one of the following exceptions:
            //   FeatureNotSupportedException
            //   FeatureNotEnabledException
            //   PermissionException
            catch (Exception ex)
            {
                throw (ex);
                // Unable to get location
            }
        }
        //Up is forward(0 degrees)
        //Down is backward(180- degrees)
        private void getArrowAngle(double bearing, double heading)
        {
            bearing = bearing * Math.PI / 180;
            heading = heading * Math.PI / 180;
            double dot = Vector2.Dot(Vector2.Normalize(new Vector2((float)Math.Cos(bearing), (float)Math.Sin(bearing))), Vector2.Normalize(new Vector2((float)Math.Cos(heading), (float)Math.Sin(heading))));
            ArrowAngle =(Math.Abs(Math.Acos(Math.Floor(dot))*180/Math.PI))%360;
        }
    }
}

