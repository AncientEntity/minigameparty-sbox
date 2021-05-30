using System;
using System.Collections.Generic;
using Sandbox;

namespace Minigames.MapVote
{
	public class MapVote
	{
		public static List<string> allowedMaps = new List<string>();
		public MapVote()
		{
			allowedMaps.Add( "gvar.flatgrass_cool" );
			allowedMaps.Add( "facepunch.construct" );
		}
	}
}
