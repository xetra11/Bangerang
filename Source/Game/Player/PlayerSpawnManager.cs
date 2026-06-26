using System.Collections.Generic;
using FlaxEngine;
using FlaxEngine.Networking;

namespace Game.Game.Player;

public class PlayerSpawn : Script
{
    public Prefab PlayerPrefab;

    private readonly Queue<uint> _pendingClients = new();
    private readonly Queue<PendingOwnershipTransfer> _pendingOwnershipTransfers = new();

    private struct PendingOwnershipTransfer
    {
        public Actor Player;
        public uint ClientId;

        public PendingOwnershipTransfer(Actor player, uint clientId)
        {
            Player = player;
            ClientId = clientId;
        }
    }

    public override void OnEnable()
    {
        if (!FlaxEngine.Networking.NetworkManager.IsServer && !FlaxEngine.Networking.NetworkManager.IsHost)
            return;

        FlaxEngine.Networking.NetworkManager.ClientConnected += OnClientConnected;

        if (FlaxEngine.Networking.NetworkManager.IsHost)
        {
            // Host/local player stays server-owned.
            // OwnerClientId = 0 / OwnedAuthoritative is correct for the host.
            SpawnHostPlayer(FlaxEngine.Networking.NetworkManager.LocalClientId);
        }
    }

    public override void OnDisable()
    {
        FlaxEngine.Networking.NetworkManager.ClientConnected -= OnClientConnected;

        _pendingClients.Clear();
        _pendingOwnershipTransfers.Clear();
    }

    public override void OnUpdate()
    {
        ProcessPendingOwnershipTransfers();

        while (_pendingClients.Count > 0)
            SpawnRemoteClientPlayer(_pendingClients.Dequeue());
    }

    private void OnClientConnected(NetworkClient client)
    {
        // StartHost owns its local player already.
        // This event is only used to create pawns for remote clients.
        if (client.ClientId == FlaxEngine.Networking.NetworkManager.LocalClientId)
            return;

        // Process this on the next game update. ClientConnected is raised while
        // the connection packet is still being handled by the network tick.
        _pendingClients.Enqueue(client.ClientId);
    }

    private void SpawnHostPlayer(uint ownerClientId)
    {
        var player = PrefabManager.SpawnPrefab(PlayerPrefab, Transform);

        player.Name = $"Player {ownerClientId}";

        // Spawn server-owned.
        NetworkReplicator.SpawnObject(player);

        // Flax 1.12 workaround:
        // keep at least one object registered so SpawnQueue processing is not blocked
        // by an empty Objects collection.
        // NetworkReplicator.AddObject(player);
    }

    private void SpawnRemoteClientPlayer(uint ownerClientId)
    {
        var player = PrefabManager.SpawnPrefab(PlayerPrefab, Transform);
        player.Name = $"Player {ownerClientId}";
        NetworkReplicator.SpawnObject(player);
        _pendingOwnershipTransfers.Enqueue(
            new PendingOwnershipTransfer(player, ownerClientId)
        );
    }

    private void ProcessPendingOwnershipTransfers()
    {
        var count = _pendingOwnershipTransfers.Count;

        for (var i = 0; i < count; i++)
        {
            var pending = _pendingOwnershipTransfers.Dequeue();

            var playerActor = pending.Player;
            if (playerActor == null)
                continue;

            // Wait until SpawnObject has actually been processed by the network update.
            if (!NetworkReplicator.HasObject(playerActor))
            {
                _pendingOwnershipTransfers.Enqueue(pending);
                continue;
            }

            NetworkReplicator.SetObjectOwnership(
                playerActor,
                pending.ClientId,
                localRole: NetworkObjectRole.Replicated,
                hierarchical: true
            );

            var playerController = playerActor.GetScript<Player>().PlayerController;
            NetworkReplicator.SetObjectOwnership(
                playerController,
                pending.ClientId,
                localRole: NetworkObjectRole.Replicated,
                hierarchical: true
            );

        }
    }

}
