using FlaxEngine;
using FlaxEngine.Networking;

namespace Game.Game.Player;

public class NetworkedPlayer: Script
{
    public override void OnEnable()
    {
        NetworkReplicator.AddObject(this);
        if (NetworkReplicator.IsObjectOwned(Actor))
        {
            Actor.Layer = 1;
        }
    }

    public override void OnDisable()
    {
        NetworkReplicator.RemoveObject(this);
    }
}
