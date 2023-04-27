using System;

namespace OppSwap
{
	public class Room
	{
		public String Name { get; set; }
        public String Id { get; set; }

		public String[] tempholderwhileplayersdonthavenamesonserver { get; set; }

        public Player[] players { get; set; }
        public Target target { get; set; }
		Boolean gameOpen;

		public Room(String name, String id)
		{
			Name = name;
			Id = id;
		}
	}
}
