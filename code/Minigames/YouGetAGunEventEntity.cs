using System;
using Sandbox;

namespace Minigames 
{
	public class YouGetAGunEventEntity : EventEntity
	{
	public override void OnMinigameStart()
	{
		foreach(MinigamePlayer player in MinigamePlayer.allPlayers)
		{
			player.Inventory.Add( new Gun(), true );
		}
	}

	public override void WhileEvent()
	{

	}
}
}
