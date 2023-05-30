using System;

namespace OppSwap
{
	public class Room
	{
		public String Name { get; set; }
        public String Id { get; set; }

        public List<Player> players { get; set; }
        public Target target { get; set; } // whoops, prob should be caps
		public bool IsAlive { get; set; } // wether user is alive
		public Player Winner { get; set; }

		public bool started;

		public Room(String name, String id)
		{
			Name = name;
			Id = id;
			started = false;
			IsAlive = true;
		}
	}
}
