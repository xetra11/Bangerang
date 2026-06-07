using FlaxEngine;

namespace Game.Game;

public class NetworkManager: Script
{
    public override void OnStart()
    {
#if SERVER
        FlaxEngine.Networking.NetworkManager.StartHost();
        Debug.Log("NetworkManager started host");
        base.OnStart();
#endif
    }
}
