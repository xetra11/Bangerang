using FlaxEngine;
using FlaxEngine.Networking;

namespace Game.Game;

public class NetworkManager : Script
{
    public bool StartAsHost = true;

    public override void OnAwake()
    {
        Debug.Log("Starting NetworkManager...");

#if FLAX_EDITOR
        if (StartAsHost)
        {
            Debug.Log("Starting as Host...");
            if (FlaxEngine.Networking.NetworkManager.StartHost())
            {
                Debug.LogError("Failed to start NetworkManager as Host");
            }
        }
        else
        {
            Debug.Log("Starting as Client...");
            if (FlaxEngine.Networking.NetworkManager.StartClient())
            {
                Debug.LogError("Failed to start NetworkManager as Client");
            }
        }

#elif SERVER
        Debug.Log("Starting as Dedicated Server/Host...");
        if (FlaxEngine.Networking.NetworkManager.StartHost())
        {
            Debug.LogError("Failed to start NetworkManager as Host");
        }
        return;
#else
        Debug.Log("Starting as Client...");
        if (FlaxEngine.Networking.NetworkManager.StartClient())
        {
            Debug.LogError("Failed to start NetworkManager as Client");
        }
#endif
    }
}
