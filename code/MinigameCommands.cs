using System;
using Sandbox;
namespace Minigames
{
	public class Commands
	{
		[ServerCmd("endround")]
		public static void EndRound()
		{
			Log.Info( "Round Ended." );
			MinigamesGame.game.EndRound();
		}

		[ServerCmd("forceround")]
		public static void ForceRound(string index)
		{
			MinigamesGame.game.ForceMinigame(int.Parse(index));
		}

		[ServerCmd("togglepause")]
		public static void TogglePause()
		{
			MinigamesGame.game.paused = !MinigamesGame.game.paused;
		}
	}
}
