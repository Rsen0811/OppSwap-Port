﻿using System;

namespace OppSwap
{
	public class Room
	{
		public String Name { get; set; }
        public String Id { get; set; }

        public List<Player> players { get; set; }
        public Target target { get; set; }
		public bool IsAlive { get; set; } // wether user is alive

		public Room(String name, String id)
		{
			Name = name;
			Id = id;
			IsAlive = true;
		}
	}
}
