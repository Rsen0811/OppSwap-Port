using System;

namespace OppSwap
{
	public class Player
	{
		public String Name { get; set; }
		public String Id { get; set; }
		public bool IsAlive { get; set; }

		public Player()
		{
		}
	}

	public class Target : Player
	{
		public LatLong position { get; set; }
        public Target()
        {
        }
    }
}