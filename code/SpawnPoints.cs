using System;
using System.Collections.Generic;
using Sandbox;
namespace Minigames
{
	public static class SpawnPoints
	{
		public static List<Vector3> closedArenaSpawnPoints = new List<Vector3>();

		static SpawnPoints()
		{
			closedArenaSpawnPoints.Add( new Vector3( -2455, - 1536, 65 ) );
			closedArenaSpawnPoints.Add( new Vector3( -2073, -1531, 65 ) );
			closedArenaSpawnPoints.Add( new Vector3( -2081, -1030, 65 ) );
			closedArenaSpawnPoints.Add( new Vector3( -2324, -288, 65 ) );
			closedArenaSpawnPoints.Add( new Vector3( -4526, -189, 65 ) );
			closedArenaSpawnPoints.Add( new Vector3( -3624, 508, 65 ) );
			closedArenaSpawnPoints.Add( new Vector3( -3921, -1532, 65 ) );
			closedArenaSpawnPoints.Add( new Vector3( -2756, 811, 65 ) );
			closedArenaSpawnPoints.Add( new Vector3( -4564, 949, 65 ) );
			closedArenaSpawnPoints.Add( new Vector3( -2977, -1209, 65 ) );
			closedArenaSpawnPoints.Add( new Vector3( -3606, -448, 65 ) );
			closedArenaSpawnPoints.Add( new Vector3( -3188, 99, 65 ));
		}

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
			Vector3 finalVec = closedArenaSpawnPoints[MinigamesGame.random.Next( 0, closedArenaSpawnPoints.Count )];
			finalVec += new Vector3( MinigamesGame.random.Next( -50, 50 ), MinigamesGame.random.Next( -50, 50 ),0f);
			return finalVec;
		}
	}


}
