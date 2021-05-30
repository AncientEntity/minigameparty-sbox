using Sandbox;

namespace Minigames
{
	public class Minigame
	{
		public string name { get; set; }
		public string desc { get; set; }
		public EventEntity eventEntity = null;


		public Minigame() { }

		public Minigame(string name, string desc, EventEntity eventEntity)
		{
			this.name = name;
			this.desc = desc;
			this.eventEntity = eventEntity;
		}

		public void StartMinigame()
		{
			if(eventEntity == null)
			{
				return;
			}
			eventEntity.enabled = true;
			eventEntity.OnMinigameStart();
		}

		public void EndMinigame()
		{
			if ( eventEntity == null )
			{
				return;
			}
			eventEntity.enabled = false;
		}
	}

	public partial class MinigamesGame
	{
		public void InitializeMinigames()
		{
			if ( !IsServer )
			{
				minigames.Add( new Minigame( "Errorpocolypse", "Avoid the errors or you'll become one!", null ) );
			} else
			{
				minigames.Add( new Minigame( "Errorpocolypse", "Avoid the errors or you'll become one!", new ErrorEventEntity() ) );
			}
			if ( !IsServer )
			{
				minigames.Add( new Minigame( "Melon Hail", "It's raining watermelons!", null ) );
			}
			else
			{
				minigames.Add( new Minigame( "Melon Hail", "It's raining watermelons!", new MelonHailEventEntity() ) );

			}
			//minigames.Add( new Minigame( "Terrynado", "Tornado but it's a bunch of Terries!", null ) );

		}
	}

}
