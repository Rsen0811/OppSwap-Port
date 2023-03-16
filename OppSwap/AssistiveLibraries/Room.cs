using System;

namespace OppSwap
{
	public class Room
	{
		String Name { get; set; }
		String Id { get; set; }

		Player[] players { get; set; }

		Player target { get; set; }

		public Room()
		{
		}
	}
}
