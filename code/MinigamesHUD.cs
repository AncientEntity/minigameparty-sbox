using Sandbox;
using Sandbox.UI;

//
// You don't need to put things in a namespace, but it doesn't hurt.
//
namespace Minigames
{
	/// <summary>
	/// This is the HUD entity. It creates a RootPanel clientside, which can be accessed
	/// via RootPanel on this entity, or Local.Hud.
	/// </summary>
	public partial class MinigamesHUD : Sandbox.HudEntity<RootPanel>
	{
		public static MinigamesHUD root;

		public MinigamesHUD()
		{
			if ( IsClient )
			{
				root = this;
				RootPanel.StyleSheet.Load( "/ui/MinigamesHUD.scss" );
				RootPanel.AddChild<GameStats>();
				RootPanel.AddChild<ChatBox>();
				RootPanel.AddChild<Health>();
				RootPanel.AddChild<NameTags>();
			}
		}

		public class Health : Panel
		{
			public string HealthText
			{
				get { return "🩸 " + MathX.CeilToInt( Local.Pawn.Health ); }
			}
			public string ScoreText
			{
				get { return "🌟 " + MathX.CeilToInt( ((MinigamePlayer)Local.Pawn).points ); }
			}

			public Health()
			{
				SetTemplate( "/ui/HealthHUD.html" );
			}
		}

		public class GameStats : Panel
		{
			public string MinigameName
			{
				get
				{
					if ( MinigamesGame.game.currentState == MinigamesGame.gameStates.waiting )
					{
						return "Waiting...";
					}
					if ( MinigamesGame.game.currentMinigame == null )
					{
						return "Minigames";
					}
					return MinigamesGame.game.currentMinigame.name;
				}
			}
			public string MinigameDesc
			{
				get
				{
					if ( MinigamesGame.game.currentState == MinigamesGame.gameStates.waiting )
					{
						return "Round starting soon...";
					}
					if ( MinigamesGame.game.currentMinigame == null )
					{
						return "Game is loading...";
					}
					return MinigamesGame.game.currentMinigame.desc;
				}
			}
			public string RoundText { 
				get {
					return "Round:    "+MinigamesGame.game.roundNumber + "/"+MinigamesGame.game.maxRounds+"   Time Remaining: "+TimeFormatted( (int)MinigamesGame.game.GetTimeLeft()); 
				}
			}

			public string TimeFormatted(int seconds )
			{
				return string.Format( "{0}:{1:D2}", seconds / 60, seconds % 60 );
			}

			public GameStats()
			{
				SetTemplate( "/ui/MinigamesHUD.html" );
			}
		}
	}

}
