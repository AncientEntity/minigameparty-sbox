using Sandbox;
using Sandbox.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Minigames
{
	partial class MinigamePlayer : Player
	{

		public static List<MinigamePlayer> allPlayers = new List<MinigamePlayer>();
		public static List<MinigamePlayer> living = new List<MinigamePlayer>();
		[Net]
		public int points { get; set; }
		public ICamera lastCam = null;

		public MinigamePlayer()
		{
			if(!allPlayers.Contains(this)) {
				allPlayers.Add( this );
			}
			Inventory = new BaseInventory(this);
			SetupPhysicsFromModel( PhysicsMotionType.Static, false );
		}

		public override void Respawn()
		{
			SetModel( "models/citizen/citizen.vmdl" );

			//
			// Use WalkController for movement (you can make your own PlayerController for 100% control)
			//
			Controller = new WalkController();

			//
			// Use StandardPlayerAnimator  (you can make your own PlayerAnimator for 100% control)
			//
			Animator = new StandardPlayerAnimator();

			//
			// Use ThirdPersonCamera (you can make your own Camera for 100% control)
			//
			if ( lastCam == null )
			{
				Camera = new FirstPersonCamera();
				lastCam = Camera;
			} else
			{
				Camera = lastCam;
			}

			EnableAllCollisions = true;
			EnableDrawing = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;
			Dress();
			//Inventory.SetActiveSlot( 0, true );
			base.Respawn();

			if(IsServer)
			{
				living.Add( this );
			}
		}

		/// <summary>
		/// Called every tick, clientside and serverside.
		/// </summary>
		public override void Simulate( Client cl )
		{
			if ( Input.Pressed( InputButton.View ) && LifeState == LifeState.Alive)
			{
				if ( Camera is FirstPersonCamera )
				{
					Camera = new ThirdPersonCamera();
				}
				else
				{
					Camera = new FirstPersonCamera();
				}
			}
			TickPlayerUse();
			SimulateActiveChild( cl, ActiveChild );
			if ( IsServer && LifeState == LifeState.Dead)
			{
				if ( MinigamesGame.game.currentState == MinigamesGame.gameStates.waiting )
				{
					Respawn();
				}
			}

			var controller = GetActiveController();
			controller?.Simulate( cl, this, GetActiveAnimator() );
			//If we're running serverside and Attack1 was just pressed, spawn a ragdoll


			//if ( IsServer && Input.Pressed( InputButton.Attack1 ) )
			//{
			//	var ragdoll = new MelonHailEventEntity.MelonEntity();
			//	ragdoll.SetModel( "models/sbox_props/watermelon/watermelon.vmdl" );
			//	ragdoll.Position = EyePos + EyeRot.Forward * 40;
			//	ragdoll.Rotation = Rotation.LookAt( Vector3.Random.Normal );
			//	ragdoll.SetupPhysicsFromModel( PhysicsMotionType.Dynamic, false );
			//	ragdoll.PhysicsGroup.Velocity = EyeRot.Forward * 1000;
			//}
			//FindUsable();
		}


		//protected override Entity FindUsable()
		//{
		//	var tr = Trace.Ray( EyePos, EyePos + EyeRot.Forward * 85 )
		//		.Radius( 2 )
		//		.HitLayer( CollisionLayer.Debris )
		//		.Ignore( this )
		//		.Run();

		//	if ( tr.Entity == null ) return null;
		//	if ( tr.Entity is not IUse use ) return null;
		//	if ( !use.IsUsable( this ) ) return null;
		//	ActiveChild = tr.Entity;
		//	return tr.Entity;
		//}

		public override void OnKilled()
		{
			base.OnKilled();
			lastCam = Camera;
			Camera = new SpectateRagdollCamera();
			Controller = new NoclipController();

			Inventory.DeleteContents();

			EnableAllCollisions = false;

			RemoveClothes();
			SetModel( "models/citizen/clothes/ghost.vmdl_c" );
			//EnableDrawing = false;

			if(IsServer && living.Contains( this ) )
			{
				GrantPoints();
				living.Remove( this );
				if(living.Count == MinigamesGame.game.currentMinigame.minPlayers-1)
				{
					MinigamesGame.game.EndRound();
				}
			}
		}

		public override void TakeDamage( DamageInfo info )
		{
			base.TakeDamage( info );
			
		}

		public void GrantPoints()
		{
			if ( living.Count == 1 )
			{
				points += 3;
			}
			else if ( living.Count == 2 )
			{
				points += 2;
			}
			else if ( living.Count == 1 )
			{
				points += 1;
			}
			AnnouncePlacement( living.Count );
		}

		[ClientRpc]
		public void AnnouncePlacement(int placement)
		{
			string suffix = "";
			if ( placement == 1 )
				suffix = "st";
			else if (placement == 4 || placement >= 5 )
				suffix = "th";
			else if ( placement == 2 )
				suffix = "nd";
			else if ( placement == 3 )
				suffix = "rd";
			ChatBox.AddChatEntry("Minigames",this.Owner.ToString().Split("/")[1] + " got "+placement+suffix+" place.");
		}
	}
}
