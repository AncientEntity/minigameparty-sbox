using Sandbox;

namespace Minigames
{
	public class Minigame
	{
		public string name { get; set; }
		public string desc { get; set; }
		public int minPlayers { get; set; }

		public int roundTime { get; set; } = 60;

		public spawnZones spawnZone { get; set; }
		public EventEntity eventEntity = null;


		public Minigame() { }

		public Minigame(string name, string desc, EventEntity eventEntity, int minPlayers = 1, spawnZones zone = spawnZones.openArea, int roundTime=60)
		{
			this.name = name;
			this.desc = desc;
			this.minPlayers = minPlayers;
			this.spawnZone = zone;
			this.eventEntity = eventEntity;
			this.roundTime = roundTime;
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
			} else if (spawnZone == spawnZones.custom)
			{
				return eventEntity.CustomSpawnPosition();
			}
			return SpawnPoints.RandomOpenAreaSpawnPoint();
		}

		public enum spawnZones
		{
			waiting,
			openArea,
			closedArea,
			custom,
		}
	}

	public partial class MinigamesGame
	{
		[Event("hotloaded")]
		public void InitializeMinigames()
		{
			minigames = new System.Collections.Generic.List<Minigame>();

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
				minigames.Add( new Minigame( "You Get A Gun!", "You know what to do...", null, 2, Minigame.spawnZones.closedArea, 300 ) );
			}
			else
			{

				minigames.Add( new Minigame( "You Get A Gun!", "You know what to do...", new YouGetAGunEventEntity(), 2, Minigame.spawnZones.closedArea, 300) );
			}

			//WARNING
			//WARNING - RENAMING GUN SPLEEF TO SOMETHING ELSE WILL BREAK GUN()'s UNLIMITED AMMO/NO PLAYER DAMAGE.
			//WARNING
			if ( !IsServer )
			{
				minigames.Add( new Minigame( "Gun Spleef", "Shoot The Boxes!", null, 2, Minigame.spawnZones.custom,300) );
			}
			else
			{
				minigames.Add( new Minigame( "Gun Spleef", "Shoot The Boxes!", new GunSpleefEventEntity(), 2, Minigame.spawnZones.custom, 300 ) );
			}


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
