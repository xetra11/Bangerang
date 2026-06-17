using FlaxEngine;
using FlaxEngine.Networking;

namespace Game.Game;

public class NetworkManager : Script
{
    public override void OnAwake()
    {
        Debug.Log("Starting NetworkManager...");
#if FLAX_EDITOR || SERVER
        if (!FlaxEngine.Networking.NetworkManager.StartHost())
        {
            Debug.LogError("Failed to start NetworkManager as Host");
        }
        return;
#endif
#if !SERVER
        if (!FlaxEngine.Networking.NetworkManager.StartClient())
        {
            Debug.LogError("Failed to start NetworkManager as Client");
        }
#endif
    }
}
