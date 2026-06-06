using FlaxEngine;
using FlaxEngine.Networking;

namespace Game.Game.Player;

public class PlayerSpawn : Script
{
    public Prefab PlayerPrefab;

    public override void OnStart()
    {
#if SERVER
        var player = PrefabManager.SpawnPrefab(PlayerPrefab, Transform);
        NetworkReplicator.SpawnObject(player);
#endif
    }
}
