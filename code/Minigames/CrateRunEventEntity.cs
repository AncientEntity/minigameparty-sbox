using System;
using Sandbox;
namespace Minigames
{
	public class CrateRunEventEntity : EventEntity
	{
		public Vector3 centerPos;
		public string crateModel = "models/citizen_props/crate01.vmdl";
		public float offsetPerCrate = 37.5f;

		public CrateRunEventEntity()
		{
			centerPos = new Vector3( 3776, 448, 768 );
		}

		public void SpawnCrateFloor(float offset)
		{
			for(int x = 0; x < 20; x++ )
			{
				for(int y = 0; y < 20; y++ )
				{
					EventProp newCrate = new EventProp();
					newCrate.SetModel( crateModel );
					newCrate.PhysicsBody.MotionEnabled = false;
					newCrate.Position = new Vector3( centerPos.x+x*offsetPerCrate, centerPos.y+y*offsetPerCrate, centerPos.z+offset );
				}
			}
		}


		public override void OnMinigameStart()
		{
			SpawnCrateFloor( 0f );
			SpawnCrateFloor( 400f );
			SpawnCrateFloor( 800f );
			foreach(MinigamePlayer player in MinigamePlayer.allPlayers)
			{
				Gun gun = new Gun();
				player.Inventory.Add(gun,true);
			}
		}

		public override void WhileEvent()
		{

		}
	}
}
