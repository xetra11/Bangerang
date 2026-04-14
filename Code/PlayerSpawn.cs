using Sandbox;

public sealed class PlayerSpawn : Component
{
	[Property] private GameObject Player;

	protected override void OnStart()
	{
		Player.Clone( WorldPosition );
	}
}
