using FlaxEngine;
using FlaxEngine.Networking;

namespace Game.Game.Player;

public class PlayerSpawn : Script
{
    public Prefab PlayerPrefab;

    public override void OnStart()
    {
        var player = PrefabManager.SpawnPrefab(PlayerPrefab, Transform);
        Debug.Log("Player spawned: " + player.Name);
    }
}
