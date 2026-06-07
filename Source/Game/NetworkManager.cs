using FlaxEngine;

namespace Game.Game;

public class NetworkManager : Script
{
    private float _timer = 0;

    public override void OnStart()
    {
#if SERVER
        FlaxEngine.Networking.NetworkManager.StartHost();
        Debug.Log("NetworkManager started as Host");
#else
        FlaxEngine.Networking.NetworkManager.StartClient();
        Debug.Log("NetworkManager started as Client");
#endif
        base.OnStart();
    }

    public override void OnUpdate()
    {
        // _timer -= Time.DeltaTime;
        // if (_timer < 0)
        // {
        //     _timer = 3;
        //     Debug.Log($"Network is {FlaxEngine.Networking.NetworkManager.State}");
        // }
        //
        base.OnUpdate();
    }
}
