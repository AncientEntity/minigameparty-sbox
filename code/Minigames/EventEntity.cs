using Sandbox;
using System;

namespace Minigames
{
	public abstract class EventEntity : Entity
	{
		public bool enabled { get; set; }

		public abstract void OnMinigameStart();
		public abstract void WhileEvent();
		public virtual Vector3 CustomSpawnPosition()
		{
			return Vector3.Zero;
		}

	}

	public class EventProp : Prop
	{
		public EventProp() : base()
		{
			MinigamesGame.RegisterEventEntity( this );
		}


		public override void OnKilled()
		{
			if ( LifeState != LifeState.Alive )
				return;
			EnableAllCollisions = false;
			PhysicsBody.MotionEnabled = true;
		}
	}
}
