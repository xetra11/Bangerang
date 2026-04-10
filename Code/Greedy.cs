using Sandbox;

public sealed class Greedy : Component, Component.ITriggerListener
{
	[Property] public int GreedValue { get; set; }

	protected override void OnUpdate()
	{
	}

	public void OnTriggerEnter( Collider other )
	{
		Log.Info( $"Entered trigger with: {other.GameObject.Name}" );
		DestroyGameObject();
	}

}
