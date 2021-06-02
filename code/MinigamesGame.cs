
using Sandbox;
using Sandbox.UI;
using System;
using System.Collections.Generic;

//
// You don't need to put things in a namespace, but it doesn't hurt.
//
namespace Minigames
{

	/// <summary>
	/// This is your game class. This is an entity that is created serverside when
	/// the game starts, and is replicated to the client. 
	/// 
	/// You can use this to create things like HUDs and declare which player class
	/// to use for spawned players.
	/// 
	/// Your game needs to be registered (using [Library] here) with the same name 
	/// as your game addon. If it isn't then we won't be able to find it.
	/// </summary>
	[Library( "minigameparty" )]
	public partial class MinigamesGame : Sandbox.Game
	{
		public static MinigamesGame game = null;
		public static Random random = new Random();
		private static List<Entity> eventEntities = new List<Entity>();

		[Net]
		public bool paused { get; set; } = false;

		//Game settings
		[Net]
		public int maxRounds { get; set; }
		public float roundDuration
		{
			get
			{
				return currentMinigame.roundTime;
			}
		}
		public float waitingDuration = 5f;
		public List<Minigame> minigames = new List<Minigame>();

		//Current Game Data
		[Net]
		public gameStates currentState { get; set; }
		[Net]
		public int roundNumber { get; set; }
		//[Net]
		//public float timeLeft { get; set; }
		[Net]
		TimeSince timeLeft { get; set; }
		[Net]
		public int currentMinigameIndex { get; set; }
		public Minigame currentMinigame { get { return minigames[currentMinigameIndex]; } }

		private int forcedMinigame = -1;

		public MinigamesGame()
		{
			if(game == null)
			{
				game = this;
			}

			if ( IsServer )
			{
				Log.Info( "My Gamemode Has Created Serverside!" );

				if(maxRounds == 0)
				{
					maxRounds = 10;
				}
				currentState = gameStates.waiting;
				timeLeft = 0;

				// Create a HUD entity. This entity is globally networked
				// and when it is created clientside it creates the actual
				// UI panels. You don't have to create your HUD via an entity,
				// this just feels like a nice neat way to do it.
				new MinigamesHUD();
			}
			InitializeMinigames();
			if ( IsClient )
			{
				Log.Info( "My Gamemode Has Created Clientside!" );
			}

		}

		/// <summary>
		/// A client has joined the server. Make them a pawn to play with
		/// </summary>
		public override void ClientJoined( Client client )
		{
			base.ClientJoined( client );

			var player = new MinigamePlayer();
			client.Pawn = player;

			player.Respawn();
			MinigamePlayer.living.Remove( player );
			player.OnKilled();

			if ( currentState == gameStates.waiting )
			{
				player.Respawn();
			}
		}

		public override void ClientDisconnect( Client cl, NetworkDisconnectionReason reason )
		{
			base.ClientDisconnect( cl, reason );
			foreach(MinigamePlayer player in MinigamePlayer.living.ToArray())
			{
				if(player == null || !player.IsValid())
				{
					MinigamePlayer.living.Remove( player );
					MinigamePlayer.allPlayers.Remove( player );
					if ( MinigamePlayer.living.Count <= currentMinigame.minPlayers - 1 )
					{
						EndRound();
					}
				}
			}
		}

		public enum roundStates
		{
			waiting,
			running,
		}
		public override void Simulate( Client cl )
		{
			base.Simulate( cl );
			if(IsServer)
			{
				RoundManagement();
			}

		}

		void RoundManagement()
		{
			if(paused)
			{
				return;
			}
			//Round must be happening
			if ( GetTimeLeft() <= 0 )
			{
				//Start a new round and exit waiting mode.
				if ( currentState == gameStates.waiting )
				{
					timeLeft = 0;
					roundNumber++;
					PickNewMinigame();
					if (forcedMinigame != -1)
					{
						currentMinigameIndex = forcedMinigame;
						forcedMinigame = -1;
					}
					currentMinigame.StartMinigame();
					foreach (MinigamePlayer player in MinigamePlayer.allPlayers.ToArray())
					{
						if(player == null || !player.IsValid())
						{
							MinigamePlayer.allPlayers.Remove( player );
							continue;
						}
						player.Position = currentMinigame.GetRandomSpawn();
					}
					Log.Info( "Starting next round: " + currentMinigame.name + "|" + currentMinigame.desc );
					currentState = gameStates.playing;


				} else if (currentState == gameStates.playing)
				{
					timeLeft = 0;
					currentMinigame.EndMinigame();
					currentState = gameStates.waiting;
					foreach(MinigamePlayer player in MinigamePlayer.living.ToArray())
					{
						if(player == null || !player.IsValid())
						{
							MinigamePlayer.living.Remove( player );
							continue;
						}
						player.GrantPoints();
						player.Health = 100;
						player.Inventory.DeleteContents();
						player.Position = SpawnPoints.RandomWaitingSpawnPoint();
					}
					foreach(Entity e in eventEntities)
					{
						if(e == null)
						{
							continue;
						}
						e.Delete();
					}
					eventEntities = new List<Entity>();

				}
			} else if (currentState == gameStates.playing)
			{
				currentMinigame.eventEntity.WhileEvent();
			}
			
		}

		public enum gameStates
		{
			waiting,
			playing,
		}

		public float GetTimeLeft()
		{
			if ( currentState == gameStates.waiting )
			{
				return waitingDuration - timeLeft; 
			} else if (currentState == gameStates.playing)
			{
				return roundDuration - timeLeft;
			}
			return 0;
		}

		public static void RegisterEventEntity(Entity ent)
		{
			eventEntities.Add( ent );
		}

		public void EndRound()
		{
			timeLeft = 99999;
		}

		public void ForceMinigame(int index)
		{
			forcedMinigame = index;
			EndRound();
		}

	}

}
