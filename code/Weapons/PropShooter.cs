using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minigameparty.Weapons
{
	public class PropShooter : Weapon
	{
		public override string ViewModelPath => "weapons/rust_pistol/v_rust_pistol.vmdl";

		public string propModel = "models/citizen_props/crate01.vmdl";

		TimeSince lastShot;

		public override void Spawn()
		{
			base.Spawn();
			lastShot = 999;
			SetModel( "weapons/rust_pistol/rust_pistol.vmdl" );
		}

		public override void AttackPrimary()
		{
			if(IsClient)
			{
				return;
			}

			TimeSincePrimaryAttack = 0;
			TimeSinceSecondaryAttack = 0;
			lastShot = 0;
			ShootProp();
		}


		public override bool CanPrimaryAttack()
		{
			if ( !Input.Pressed( InputButton.Attack1 ) || lastShot < 0.8f)
				return false;

			return base.CanPrimaryAttack();
		}


		void ShootProp()
		{
			var ent = new Prop
			{
				Position = Owner.EyePos + Owner.EyeRot.Forward * 50,
				Rotation = Owner.EyeRot
			};

			ent.SetModel(propModel);
			ent.Velocity = Owner.EyeRot.Forward * 3000;
		}


	}

	public class CanShooter : PropShooter
	{
		
		public CanShooter()
		{
			propModel = "models/citizen_props/sodacan01.vmdl_c";
		}

	}
}
