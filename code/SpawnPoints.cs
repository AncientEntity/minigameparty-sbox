using System;
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
	}
}
