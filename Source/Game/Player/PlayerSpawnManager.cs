using System.Collections.Generic;
using FlaxEngine;
using FlaxEngine.Networking;

namespace Game.Game.Player;

public class PlayerSpawn : Script
{
    public Prefab PlayerPrefab;

    private readonly Queue<uint> _pendingClients = new();

    public override void OnEnable()
    {
        if (!FlaxEngine.Networking.NetworkManager.IsServer &&
            !FlaxEngine.Networking.NetworkManager.IsHost)
            return;

        FlaxEngine.Networking.NetworkManager.ClientConnected += OnClientConnected;

        if (FlaxEngine.Networking.NetworkManager.IsHost)
            SpawnPlayer(
                FlaxEngine.Networking.NetworkManager.LocalClientId,
                NetworkObjectRole.OwnedAuthoritative
            );
    }

    public override void OnDisable()
    {
        FlaxEngine.Networking.NetworkManager.ClientConnected -= OnClientConnected;
    }

    public override void OnUpdate()
    {
        while (_pendingClients.Count > 0)
            SpawnPlayer(_pendingClients.Dequeue(), NetworkObjectRole.Replicated);
    }

    private void OnClientConnected(NetworkClient client)
    {
        // StartHost owns its local player already. This event is only used
        // to create pawns for remote clients.
        if (client.ClientId == FlaxEngine.Networking.NetworkManager.LocalClientId)
            return;

        // Process this on the next game update. ClientConnected is raised while
        // the connection packet is still being handled by the network tick.
        _pendingClients.Enqueue(client.ClientId);
    }

    private void SpawnPlayer(uint ownerClientId, NetworkObjectRole localRole)
    {
        var player = PrefabManager.SpawnPrefab(PlayerPrefab, Transform);

        // Flax 1.12 does not process SpawnQueue while the registered object
        // collection is empty. Queue ownership first, then register the actor.
        NetworkReplicator.SpawnObject(player);
        NetworkReplicator.SetObjectOwnership(player, ownerClientId, localRole);
        NetworkReplicator.AddObject(player);

        player.GetScript<NetworkedPlayer>()?.TryConfigureOwnership();
    }
}
