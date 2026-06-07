using FlaxEngine;
using FlaxEngine.Networking;

namespace Game.Game;

public class NetworkManager : Script
{
    public override void OnAwake()
    {
        Debug.Log("Starting NetworkManager...");
#if !BUILD_RELEASE
        NetworkReplicator.EnableLog = true;
#endif
        FlaxEngine.Networking.NetworkManager.StateChanged += OnNetworkStateChanged;
#if SERVER
        if (FlaxEngine.Networking.NetworkManager.StartHost())
        {
            Debug.LogError("Failed to start NetworkManager as Host");
        }
#else
        if (FlaxEngine.Networking.NetworkManager.StartClient())
        {
            Debug.LogError("Failed to start NetworkManager as Client");
        }
#endif
    }

    public override void OnDestroy()
    {
        FlaxEngine.Networking.NetworkManager.StateChanged -= OnNetworkStateChanged;
    }

    private void OnNetworkStateChanged()
    {
        Debug.Log(
            $"Network state changed: mode={FlaxEngine.Networking.NetworkManager.Mode}, " +
            $"state={FlaxEngine.Networking.NetworkManager.State}, " +
            $"localClient={FlaxEngine.Networking.NetworkManager.LocalClientId}"
        );
    }
}
