using System;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics;
using System.Numerics;
namespace OppSwap.ViewModels
{
    [QueryProperty(nameof(CurrRoom)/*The name of the property in ViewModel*/, nameof(CurrRoom)/*The name of the property when you switch pages*/)]

    public partial class RoomDetailPageViewModel : ObservableObject
    {

        [ObservableProperty]
        Room currRoom;

        [ObservableProperty]
        String timeTaken;

        [ObservableProperty]
        String latitudeLongitude;

        [ObservableProperty]
        String currHeading;

        [ObservableProperty]
        double arrowAngle;

        [ObservableProperty]
        bool buttonVisible = false;

        [ObservableProperty]
        String targetName;


        LatLong location;
        LatLong pole = new LatLong();


        LatLong pos;

        public RoomDetailPageViewModel()
        {
            //CurrRoom = new Room("bruh", "bruh");

            pos = pole;//Room.target.position;
            latitudeLongitude = "0 , 0";
            TimeTaken = "0";

            //assumed direction you are facing
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
                
                //timer code move the stop and the elapsed milliseconds to after the l declaration
                //Stopwatch stopwatch = new Stopwatch();
                //stopwatch.Start();
                //TimeTaken = stopwatch.ElapsedMilliseconds + "";
                //stopwatch.Stop();
                //TODO REMOVE THIS LOCATION FINDING IN THE FUTURE

                //TODO HIGH PRIORITY pls add suppourt for heading.
                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(30));
                Location l = await Geolocation.Default.GetLocationAsync(request);



                location = new LatLong(l.Latitude, l.Longitude);
                ClientInterconnect.UpdatePosition(location);
                //in the future keep in mind to create a check for a mock location provider so we can do something if it is a faked location

                ClientInterconnect.c.GetTargetPos(CurrRoom.Id);
                await Task.Delay(500);
                LatitudeLongitude = location.ToString();
                pos = ClientInterconnect.getTargetPos(CurrRoom.Id);


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
            double dot = Vector2.Dot(Vector2.Normalize(new Vector2((float)Math.Cos(bearing), (float)Math.Sin(bearing))),
                Vector2.Normalize(new Vector2((float)Math.Cos(heading), (float)Math.Sin(heading))));
            ArrowAngle = (Math.Abs(Math.Acos(Math.Floor(dot)) * 180 / Math.PI)) % 360;
        }

        [ObservableProperty]
        String target;

        void updateTarget(Room game)
        {
            TargetName = game.target.Name;
        }
        [RelayCommand]
        public async Task Update()
        {
            await Task.Delay(1000);
            while (true)
            {
                //CurrRoom = ClientInterconnect.getRoom(CurrRoom.Id);
                ClientInterconnect.GetTargetPos(CurrRoom.Id);

                await Task.Delay(1500);

                TimeTaken = "" + ClientInterconnect.position.distance(CurrRoom.target.Position);

                if (ClientInterconnect.position.distance(CurrRoom.target.Position) <= 50)
                {
                    ButtonVisible = true;
                }
                else
                {
                    ButtonVisible = false;
                }
                LatitudeLongitude=ClientInterconnect.position.ToString();

                //TODO check if this works
                ArrowAngle=ClientInterconnect.position.bearing(CurrRoom.target.Position);
            }
        }

        [RelayCommand]
        public void KillButton()
        {
            //kill code
            if (ButtonVisible)
            {
                ClientInterconnect.Kill(CurrRoom.Id);
            }
            
        }

    }
    
}

