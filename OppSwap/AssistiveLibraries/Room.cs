using System;

namespace OppSwap
{
	public class Room
	{
		String Name { get; set; }
		String Id { get; set; }

		Player[] players { get; set; }
		Target target { get; set; }

		public Room(String name, String id)
		{
			Name = name;
			Id = id;
		}
	}
}
