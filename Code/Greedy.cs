using Sandbox;
using Sandbox.Diagnostics;

public sealed class Greedy : Component, Component.ITriggerListener
{
	[Property] public int GreedValue { get; set; }

	protected override void OnUpdate()
	{
	}

	public void OnTriggerEnter( Collider other )
	{
		Component player = other.GameObject.GetComponentInParent<Player>();
		if (!player.IsValid) return;
		DestroyGameObject();
	}

}
