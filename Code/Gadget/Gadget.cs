using System;
using Sandbox;
using Sandbox.Gadget;

public sealed class Gadget : Component
{
	[Property] public String Name { get; set; }
	[Property] public Projectile Projectile { get; set; }

	public void UseGadget( Vector3 playerLocation )
	{
		var projectileInstance = Projectile.GameObject.Clone( playerLocation );
		Log.Info( $"Spawned projectile at {playerLocation}" );
	}

	protected override void OnUpdate()
	{
	}
}
