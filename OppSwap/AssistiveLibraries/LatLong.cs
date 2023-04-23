using System;
namespace OppSwap
{
    public class LatLong
    {
        private double latitude;
        private double longitude;
        public LatLong(double latitude, double longitude)
        {
            this.latitude = latitude;
            this.longitude = longitude;
        }
        public LatLong()
        {
            this.latitude = -90;
            this.longitude = -45;
        }

        public LatLong(String inputString)
        {
            String[] data = inputString.Split(",");
            this.latitude = Double.Parse(data[0]);
            this.longitude = Double.Parse(data[1]);
        }

        public String ToString()
        {
            return $"{latitude},{longitude}";
        }

        //https://www.movable-type.co.uk/scripts/latlong.html
        //the link for the math behind bearing and distance methods
        public double bearing(LatLong other)
        {
            double R = 6371000; // metres
            double phi1 = this.latitude * Math.PI / 180; // phi, lambda in radians
            double phi2 = other.latitude * Math.PI / 180;
            double Δlambda = (other.longitude - this.longitude) * Math.PI / 180;
            double y = Math.Sin(Δlambda) * Math.Cos(phi2);
            double x = Math.Cos(phi1) * Math.Sin(phi2) -
                      Math.Sin(phi1) * Math.Cos(phi2) * Math.Cos(Δlambda);
            double angle = Math.Atan2(y, x);
            double bearing = (angle * 180 / Math.PI + 360) % 360; // in degrees
            return bearing;
        }
        public double distance(LatLong other)
        {
            double R = 6371000; // metres
            double phi1 = this.latitude * Math.PI / 180; // phi, lambda in radians
            double phi2 = other.latitude * Math.PI / 180;
            double Δphi = (other.latitude - this.latitude) * Math.PI / 180;
            double Δlambda = (other.longitude - this.longitude) * Math.PI / 180;

            double a = Math.Sin(Δphi / 2) * Math.Sin(Δphi / 2) +
                      Math.Cos(phi1) * Math.Cos(phi2) *
                      Math.Sin(Δlambda / 2) * Math.Sin(Δlambda / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            double d = R * c; // in metres
            return d;
        }
    }
}
