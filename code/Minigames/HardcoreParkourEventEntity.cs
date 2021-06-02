using System;
using System.Collections.Generic;
using Sandbox;
namespace Minigames
{
	public class HardcoreParkourEventEntity : EventEntity
	{
		public Vector3 centerPos;
		public string crateModel = "models/citizen_props/crate01.vmdl";
		public Vector2 randomOffset = new Vector2( 38*3,100*3);

		private List<ModelEntity> allCrates = new List<ModelEntity>();
		private List<ModelEntity> startCrates = new List<ModelEntity>();
		private List<ModelEntity> endCrates = new List<ModelEntity>();

		private int curPos = 1;

		private TimeSince eventStarted;

		public HardcoreParkourEventEntity()
		{
			centerPos = new Vector3( 3776, 448, 768 );
		}

		public void SpawnParkourCourse(int parkourLength=10)
		{
			float totalOffsetX = 0f;
			float totalOffsetY = 0f;
			for(int i = 0; i < parkourLength; i++ ) {
				ModelEntity newCrate = new ModelEntity();
				newCrate.SetModel( crateModel );
				newCrate.SetupPhysicsFromModel( PhysicsMotionType.Static );
				float randomDistance = MinigamesGame.random.Next((int)randomOffset.x,(int)randomOffset.y);
				float a = MinigamesGame.random.Next( 15, (int)randomDistance - 15 );
				float b = MathF.Sqrt(randomDistance*randomDistance-a*a);
				if ( MinigamesGame.random.Next( 0, 2 ) == 1 )
				{
					b = -b;
				}
				//Log.Info(""+ randomDistance+"|" + a + "|" + b );
				newCrate.Position = new Vector3( a + centerPos.x + totalOffsetX, b + centerPos.y + totalOffsetY, centerPos.z + MinigamesGame.random.Next(-15,15));
				newCrate.Scale = 1.5f;
				
				if(i == 0 || i == parkourLength-1)
				{
					//first and last
					float offset = -38 * newCrate.Scale;
					newCrate.Delete();
					for ( int j = 0; j < 10; j++ )
					{
						ModelEntity extraCrate = new ModelEntity();
						extraCrate.SetModel( crateModel );
						extraCrate.SetupPhysicsFromModel( PhysicsMotionType.Static );
						extraCrate.Position = newCrate.Position + new Vector3(0f, offset + j*65, 0f);
						extraCrate.Scale = 1.5f;
						extraCrate.RenderColor = Color.Red;
						allCrates.Add( extraCrate );
						MinigamesGame.RegisterEventEntity( extraCrate );
						if(i == 0)
						{
							startCrates.Add( extraCrate );
						} else
						{
							endCrates.Add( extraCrate );
						}
					}
				}

				totalOffsetX += a;
				totalOffsetY += b;
				allCrates.Add( newCrate );
				MinigamesGame.RegisterEventEntity( newCrate );
				
			}
		}


		public override void OnMinigameStart()
		{
			eventStarted = 0;
			allCrates = new List<ModelEntity>();
			startCrates = new List<ModelEntity>();
			endCrates = new List<ModelEntity>();
			curPos = 1;
			SpawnParkourCourse( 20 );
			foreach ( MinigamePlayer player in MinigamePlayer.allPlayers )
			{
				//Gun gun = new Gun();
				//player.Inventory.Add( gun, true );
				player.Velocity = Vector3.Zero; //Try to prevent people from running off the boxes when they first teleport.
			}
		}

		public override Vector3 CustomSpawnPosition()
		{
			return startCrates[MinigamesGame.random.Next( 0, startCrates.Count )].Position + new Vector3( 0f, 0f, 50f );
		}

		public override void WhileEvent()
		{
			foreach ( MinigamePlayer player in MinigamePlayer.living.ToArray() )
			{
				if ( player.Position.z <= -269 )
				{
					player.Position = CustomSpawnPosition();
					player.Velocity = 0f;
				} 
				if (player.Position.z >= endCrates[0].Position.z && player.Position.x >= endCrates[0].Position.x)
				{
					player.TakeDamage( DamageInfo.Generic( 99999f ) );
					curPos += 1;
				}
				if(eventStarted > 2f)
				{
					player.isFrozen = false;
				}
			}
		}
	}
}
