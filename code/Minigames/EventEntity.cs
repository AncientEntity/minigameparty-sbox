using Sandbox;
using System;

namespace Minigames
{
	public abstract class EventEntity : Entity
	{
		public bool enabled { get; set; }

		public abstract void OnMinigameStart();
		public abstract void WhileEvent();

	}

	public class EventProp : Prop
	{
		public EventProp() : base()
		{
			MinigamesGame.RegisterEventEntity( this );
		}
	}
}
