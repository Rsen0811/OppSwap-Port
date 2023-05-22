using System;

namespace OppSwap
{
	public class Player
	{
		public String Name { get; set; }
		public String Id { get; set; }//TODO no need for this variable right
		public bool IsAlive { get; set; }

		public Player(String name, String id)
		{
			this.Name = name;
			this.Id = id;
		}
	}

	public class Target : Player
	{
		public LatLong Position { get; set; }
        public Target(String name, String Id)
        {
			this.Name = name;
			this.Id = Id;
			this.Position = new LatLong(0, 0);
        }
        public Target(String name, LatLong pos)
        {
            this.Name = name;
            this.Position = pos;
        }

    }
}