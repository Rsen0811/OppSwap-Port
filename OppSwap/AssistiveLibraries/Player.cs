using System;

namespace OppSwap
{
	public class Player
	{
		public String Name { get; set; }
		public String Id { get; set; }//TODO no need for this variable right
		public bool IsAlive { get; set; }

		public Player(string id, string name)
		{
			this.Name = name;
			this.Id = id;
		}
	}

	public class Target : Player
	{
		public LatLong Position { get; set; }
        public Target(string id, string name) : base(id, name)
        {			
			this.Position = new LatLong(0, 0);
        }
    }
}