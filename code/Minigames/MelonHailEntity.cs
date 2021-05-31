using System;
using Sandbox;
using Sandbox.UI;

namespace Minigames
{
	public class MelonHailEventEntity : EventEntity
	{

		public float SPAWN_DELAY = 0.08f;

		TimeSince lastSpawn = new TimeSince();

		public MelonHailEventEntity()
		{

			lastSpawn = 0;
		}

		public override void OnMinigameStart() { }

		public override void WhileEvent()
		{
			if (!enabled || !IsServer)
			{
				return;
			}
			if(lastSpawn > SPAWN_DELAY)
			{
				SpawnMelon();
			}
		}


		private void SpawnMelon()
		{
			lastSpawn = 0;
			MelonEntity newMelon = new MelonEntity();
			newMelon.Position = new Vector3(MinigamesGame.random.Next(-1227,1601), MinigamesGame.random.Next( -1691, 1159 ), 1126 );
			newMelon.Scale *= (float)MinigamesGame.random.NextDouble() * 1f + 5f;
			newMelon.SetModel( "models/sbox_props/watermelon/watermelon.vmdl" );
			newMelon.SetupPhysicsFromModel( PhysicsMotionType.Dynamic, false ); 
			newMelon.PhysicsGroup.Velocity = Vector3.Random * ((float)MinigamesGame.random.NextDouble() * 10000f);

		}



		public class MelonEntity : ModelEntity
		{
			TimeSince timeSinceStart;

			public MelonEntity()
			{
				timeSinceStart = 0;

				MinigamesGame.RegisterEventEntity( this );
				
			}


			public override void Simulate( Client cl )
			{
				if(timeSinceStart > 3)
				{
					Delete();
				}
			}
			protected override void OnPhysicsCollision(CollisionEventData eventData)
			{
				if(PhysicsBody == null)
				{
					return;
				}

				bool debug_prop_explosion = false;

				//Go boom
				Prop EE = new Prop();
				EE.SetModel( "models/rust_props/barrels/fuel_barrel.vmdl_c" );
				ModelExplosionBehavior explosionBehavior = EE.GetModel().GetExplosionBehavior();
				EE.Delete();
				Sound.FromWorld( explosionBehavior.Sound, PhysicsBody.MassCenter + new Vector3(0f,0f,9200f));
				Particles.Create( explosionBehavior.Effect, PhysicsBody.MassCenter );


				var sourcePos = PhysicsBody.MassCenter;
				var overlaps = Physics.GetEntitiesInSphere( sourcePos, explosionBehavior.Radius );

				if ( debug_prop_explosion )
					DebugOverlay.Sphere( sourcePos, explosionBehavior.Radius, Color.Orange, true, 1 );

				foreach ( var overlap in overlaps )
				{
					if ( overlap is not ModelEntity ent || !ent.IsValid() )
						continue;

					if ( ent.LifeState != LifeState.Alive )
						continue;

					if ( !ent.PhysicsBody.IsValid() )
						continue;

					if ( ent.IsWorld )
						continue;

					var targetPos = ent.PhysicsBody.MassCenter;

					var dist = Vector3.DistanceBetween( sourcePos, targetPos );
					if ( dist > explosionBehavior.Radius )
						continue;

					var tr = Trace.Ray( sourcePos, targetPos )
						.Ignore( this )
						.WorldOnly()
						.Run();

					if ( tr.Fraction < 1.0f )
					{
						if ( debug_prop_explosion )
							DebugOverlay.Line( sourcePos, tr.EndPos, Color.Red, 5, true );

						continue;
					}

					if ( debug_prop_explosion )
						DebugOverlay.Line( sourcePos, targetPos, 5, true );

					var distanceMul = 1.0f - Math.Clamp( dist / explosionBehavior.Radius, 0.0f, 1.0f );
					var damage = explosionBehavior.Damage * distanceMul;
					var force = (explosionBehavior.Force * distanceMul) * ent.PhysicsBody.Mass;
					var forceDir = (targetPos - sourcePos).Normal;

					ent.TakeDamage( DamageInfo.Explosion( sourcePos, forceDir * force, damage )
						.WithAttacker( this ) );
				}

				Delete();
			}

		}

	}


}
