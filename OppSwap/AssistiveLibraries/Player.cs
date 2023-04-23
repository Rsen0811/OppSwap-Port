using System;

namespace OppSwap
{
	public class Player
	{
		public String Name { get; set; }
		public String Id { get; set; }//TODO no need for this variable right
		public bool IsAlive { get; set; }

		public Player()
		{
		}
	}

	public class Target : Player
	{
		public LatLong Position { get; set; }
        public Target(String name)
        {
			this.Name = name;
			this.Position = new LatLong(0, 0);
        }
        public Target(String name, LatLong pos)
        {
            this.Name = name;
            this.Position = pos;
        }

    }
}