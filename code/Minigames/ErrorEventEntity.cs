using System;
using System.Collections.Generic;
using Sandbox;

namespace Minigames
{
	public class ErrorEventEntity : EventEntity
	{
		public List<Vector3> errorPositions = new List<Vector3>();
		public List<ErrorEntity> allErrors = new List<ErrorEntity>();

		public int errorCount = 25;
		public float startingSize = 0.5f;
		public float growSpeed = 0.01f;

		public ErrorEventEntity()
		{

		}

		public override void OnMinigameStart()
		{
			allErrors = new List<ErrorEntity>();
			errorPositions = new List<Vector3>();
		}

		public override void WhileEvent()
		{
			if(allErrors.Count < errorCount)
			{
				SpawnError();
			}

			int c = 0;
			foreach(ErrorEntity ent in allErrors)
			{
				ent.Scale += growSpeed;
				ent.Position = errorPositions[c];
				c++;
			}
		}

		public void SpawnError()
		{
			ErrorEntity erEnt = new ErrorEntity();
			erEnt.SetModel( "models/dev/error.vmdl_c" );
			erEnt.Scale = startingSize;
			erEnt.Position = new Vector3( MinigamesGame.random.Next( -1227, 1601 ), MinigamesGame.random.Next( -1691, 1159 ), MinigamesGame.random.Next( -100, 720 ) );
			erEnt.Rotation = Rotation.FromAxis(new Vector3( MinigamesGame.random.Next( 0, 360 ) , MinigamesGame.random.Next( 0, 360 ) , MinigamesGame.random.Next( 0, 360 )), (float)MinigamesGame.random.NextDouble());
			erEnt.EnableAllCollisions = true;
			erEnt.SetupPhysicsFromModel( PhysicsMotionType.Dynamic, false );
			erEnt.RenderColor = Color32.Red;
			allErrors.Add( erEnt );
			errorPositions.Add( erEnt.Position );
		}

		public class ErrorEntity : ModelEntity
		{
			public ErrorEntity()
			{
				MinigamesGame.RegisterEventEntity( this );
			}

			protected override void OnPhysicsCollision( CollisionEventData eventData )
			{
				eventData.Entity.TakeDamage( DamageInfo.Generic( 99999f ) );
				
				base.OnPhysicsCollision( eventData );
			}
		}
	}
}
