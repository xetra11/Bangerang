using System;
using Sandbox;
using Sandbox.Gadget;

public sealed class Gadget : Component
{
	[Property] public String Name { get; set; }
	[Property] public GameObject ProjectilePrefab { get; set; }

	public void UseGadget( Vector3 playerLocation )
	{
		var projectileInstance = ProjectilePrefab.Clone( playerLocation );
		projectileInstance.GetComponent<Projectile>().Direction = Vector3.Backward;
		Log.Info( $"Spawned projectile at {playerLocation}" );
	}

	protected override void OnUpdate()
	{
	}
}
