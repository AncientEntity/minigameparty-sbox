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
		public float growSpeed = 0.7f;

		private TimeSince sinceEventStart;

		public ErrorEventEntity()
		{

		}

		public override void OnMinigameStart()
		{
			allErrors = new List<ErrorEntity>();
			errorPositions = new List<Vector3>();
			sinceEventStart = 0;
		}

		public override void WhileEvent()
		{
			if(allErrors.Count < errorCount)
			{
				SpawnError();
			}

			Host.AssertServer();
			int c = 0;
			float curScale = growSpeed * sinceEventStart;
			foreach(ErrorEntity ent in allErrors)
			{
				ent.Scale = curScale;
				ent.Position = errorPositions[c];
				ent.Velocity = Vector3.Zero;
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
			erEnt.PhysicsBody.GravityScale = 0f;
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
