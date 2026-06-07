using FlaxEngine;
using FlaxEngine.Networking;

namespace Game.Game.Player;

public class PlayerSpawn : Script
{
    public Prefab PlayerPrefab;

    public override void OnEnable()
    {
#if FLAX_EDITOR
        var player = PrefabManager.SpawnPrefab(PlayerPrefab, Transform);
        Debug.Log("Spawning player");
#endif
        if (FlaxEngine.Networking.NetworkManager.IsHost)
        {
            SpawnHostPlayer();
            FlaxEngine.Networking.NetworkManager.ClientConnected += OnClientConnected;
        }
    }

    private void OnClientConnected(NetworkClient client)
    {
        var player = PrefabManager.SpawnPrefab(PlayerPrefab, Transform);
        Debug.Log("Spawning player for client: " + client.ClientId);
        NetworkReplicator.SpawnObject(player);
        NetworkReplicator.SetObjectOwnership(player, client.ClientId, NetworkObjectRole.ReplicatedSimulated);
        player.GetScript<NetworkedPlayer>().OnSpawned();
    }

    private void SpawnHostPlayer()
    {
        var player = PrefabManager.SpawnPrefab(PlayerPrefab, Transform);
        Debug.Log("Spawning player for host");
        NetworkReplicator.SpawnObject(player);
        NetworkReplicator.SetObjectOwnership(player,
            FlaxEngine.Networking.NetworkManager.LocalClientId,
            NetworkObjectRole.OwnedAuthoritative);
        player.GetScript<NetworkedPlayer>().OnSpawned();
    }
}
