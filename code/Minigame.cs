using Sandbox;

namespace Minigames
{
	public class Minigame
	{
		public string name { get; set; }
		public string desc { get; set; }
		public int minPlayers { get; set; }
		public spawnZones spawnZone { get; set; }
		public EventEntity eventEntity = null;


		public Minigame() { }

		public Minigame(string name, string desc, EventEntity eventEntity, int minPlayers = 1, spawnZones zone = spawnZones.openArea)
		{
			this.name = name;
			this.desc = desc;
			this.minPlayers = minPlayers;
			this.spawnZone = zone;
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

		public Vector3 GetRandomSpawn()
		{
			if(spawnZone == spawnZones.openArea)
			{
				return SpawnPoints.RandomOpenAreaSpawnPoint();
			} else if (spawnZone == spawnZones.waiting)
			{
				return SpawnPoints.RandomWaitingSpawnPoint();
			} else if (spawnZone == spawnZones.closedArea)
			{
				return SpawnPoints.RandomClosedAreaSpawnPoint();
			}
			return SpawnPoints.RandomOpenAreaSpawnPoint();
		}

		public enum spawnZones
		{
			waiting,
			openArea,
			closedArea,
		}
	}

	public partial class MinigamesGame
	{
		public void InitializeMinigames()
		{
			if ( !IsServer )
			{
				minigames.Add( new Minigame( "Errorpocolypse", "Avoid the errors or you'll become one!", null ,1,Minigame.spawnZones.openArea) );
			}
			else
			{
				minigames.Add( new Minigame( "Errorpocolypse", "Avoid the errors or you'll become one!", new ErrorEventEntity(),1, Minigame.spawnZones.openArea ) );
			}
			if ( !IsServer )
			{
				minigames.Add( new Minigame( "Melon Hail", "It's raining watermelons!", null, 1,Minigame.spawnZones.openArea ) );
			}
			else
			{
				minigames.Add( new Minigame( "Melon Hail", "It's raining watermelons!", new MelonHailEventEntity(),1, Minigame.spawnZones.openArea ) );

			}
			if ( !IsServer )
			{
				minigames.Add( new Minigame( "You Get A Gun!", "You know what to do...", null, 2, Minigame.spawnZones.closedArea ) );
			}
			else
			{

				minigames.Add( new Minigame( "You Get A Gun!", "You know what to do...", new YouGetAGunEventEntity(), 2, Minigame.spawnZones.closedArea ) );
			}
			//minigames.Add( new Minigame( "Terrynado", "Tornado but it's a bunch of Terries!", null ) );

		}

		public void PickNewMinigame()
		{
			bool picking = true;
			while ( picking )
			{
				currentMinigameIndex = random.Next( 0, minigames.Count );
				if ( MinigamePlayer.allPlayers.Count >= currentMinigame.minPlayers )
				{
					picking = false;
				}
			}
		}
	}
}
