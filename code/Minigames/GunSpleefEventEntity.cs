using System;
using System.Collections.Generic;
using Sandbox;
namespace Minigames
{
	public class GunSpleefEventEntity : EventEntity
	{
		public Vector3 centerPos;
		public string crateModel = "models/citizen_props/crate01.vmdl";
		public float offsetPerCrate = 37.5f;

		private List<EventProp> allCrates = new List<EventProp>();

		public GunSpleefEventEntity()
		{
			centerPos = new Vector3( 3776, 448, 768 );
		}

		public void SpawnCrateFloor(float offset)
		{
			for(int x = 0; x < 25; x++ )
			{
				for(int y = 0; y < 25; y++ )
				{
					EventProp newCrate = new EventProp();
					newCrate.SetModel( crateModel );
					newCrate.SetupPhysicsFromModel( PhysicsMotionType.Dynamic );
					newCrate.PhysicsBody.MotionEnabled = false;
					newCrate.Position = new Vector3( centerPos.x+x*offsetPerCrate, centerPos.y+y*offsetPerCrate, centerPos.z+offset );
					allCrates.Add( newCrate );
				}
			}
		}


		public override void OnMinigameStart()
		{
			allCrates = new List<EventProp>();
			SpawnCrateFloor( 0f );
			SpawnCrateFloor( 400f );
			SpawnCrateFloor( 800f );
			foreach(MinigamePlayer player in MinigamePlayer.allPlayers)
			{
				Gun gun = new Gun();
				player.Inventory.Add(gun,true);
				player.Velocity = Vector3.Zero; //Try to prevent people from running off the boxes when they first teleport.
			}
		}

		public override Vector3 CustomSpawnPosition()
		{
			return new Vector3( MinigamesGame.random.Next( 3847, 4458 ) , MinigamesGame.random.Next( 522, 1128 ),1606.21f );
		}

		public override void WhileEvent()
		{
			foreach(EventProp eP in allCrates.ToArray())
			{
				if(eP == null || !eP.IsValid())
				{
					continue;
				}
				if ( eP.Position.z <= -269 )
				{
					eP.Delete();
				}
			}
			foreach(MinigamePlayer player in MinigamePlayer.living.ToArray())
			{
				if(player.Position.z <= -269)
				{
					player.TakeDamage( DamageInfo.Generic( 999999f ) );
				}
			}
		}
	}
}
