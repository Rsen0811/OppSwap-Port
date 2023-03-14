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
			this.latitude = 90;
			this.longitude = 135;
		}

        //https://www.movable-type.co.uk/scripts/latlong.html
        //the link for the math behind bearing and distance methods
        public double bearing(LatLong other)
        {
            double R = 6371000; // metres
            double φ1 = this.latitude * Math.PI / 180; // φ, λ in radians
            double φ2 = other.latitude * Math.PI / 180;
            double λ2 = other.longitude * Math.PI / 180;
            double λ1 = other.longitude * Math.PI / 180;
            double Δφ = (other.latitude - this.latitude) * System.Math.PI / 180;
            double Δλ = (other.longitude - this.longitude) * Math.PI / 180;
            double y = Math.Sin(λ2 - λ1) * Math.Cos(φ2);
            double x = Math.Cos(φ1) * Math.Sin(φ2) -
                      Math.Sin(φ1) * Math.Cos(φ2) * Math.Cos(λ2 - λ1);
            double θ = Math.Atan2(y, x);
            double bearing = (θ * 180 / Math.PI + 360) % 360; // in degrees
            return bearing;
        }
        public double distance(LatLong other)
        {
            double R = 6371000; // metres
            double φ1 = this.latitude * Math.PI / 180; // φ, λ in radians
            double φ2 = other.latitude * Math.PI / 180;
            double Δφ = (other.latitude - this.latitude) * Math.PI / 180;
            double Δλ = (other.longitude - this.longitude) * Math.PI / 180;

            double a = Math.Sin(Δφ / 2) * Math.Sin(Δφ / 2) +
                      Math.Cos(φ1) * Math.Cos(φ2) *
                      Math.Sin(Δλ / 2) * Math.Sin(Δλ / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            double d = R * c; // in metres
            return d;
        }
    }
}

