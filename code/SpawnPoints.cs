using System;
using System.Collections.Generic;
using Sandbox;
namespace Minigames
{
	public static class SpawnPoints
	{
		public static Vector3 RandomWaitingSpawnPoint()
		{
			return new Vector3( MinigamesGame.random.Next( -624, -415 ), MinigamesGame.random.Next( -2931, -2110 ), 117 );
		}

		public static Vector3 RandomOpenAreaSpawnPoint()
		{
			return new Vector3( MinigamesGame.random.Next( -720, 1305 ), MinigamesGame.random.Next( -1455, 427 ), 135 );
		}

		public static Vector3 RandomClosedAreaSpawnPoint()
		{
			Vector3 finalVec = SpawnPointClosedArea.allSpawns[MinigamesGame.random.Next( 0, SpawnPointClosedArea.allSpawns.Count )].Position;
			finalVec += new Vector3( MinigamesGame.random.Next( -50, 50 ), MinigamesGame.random.Next( -50, 50 ),0f);
			return finalVec;
		}
	}

	[Library("spawn_closedArea")]
	public class SpawnPointClosedArea : Entity
	{
		public static List<SpawnPointClosedArea> allSpawns = new List<SpawnPointClosedArea>();

		public SpawnPointClosedArea()
		{
			allSpawns.Add( this );
		}
	}


}
