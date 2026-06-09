using FlaxEngine;
using FlaxEngine.Networking;

namespace Game.Game;

public class NetworkManager : Script
{
    public override void OnAwake()
    {
        Debug.Log("Starting NetworkManager...");
#if FLAX_EDITOR || SERVER
        if (FlaxEngine.Networking.NetworkManager.StartHost())
        {
            Debug.LogError("Failed to start NetworkManager as Host");
            NetworkReplicator.EnableLog = true;
        }
        return;
#endif
#if !SERVER
        if (FlaxEngine.Networking.NetworkManager.StartClient())
        {
            Debug.LogError("Failed to start NetworkManager as Client");
            NetworkReplicator.EnableLog = true;
        }
#endif
    }
}
