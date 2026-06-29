using FlaxEngine;
using FlaxEngine.Networking;

namespace Game.Game.Player;

public class PlayerActorTransformSyncer: Script
{
    public Actor MainTransform;
    public Actor RigidPlayer;

    public override void OnUpdate()
    {
        if (!NetworkReplicator.HasObject(this) ||
            !NetworkReplicator.IsObjectOwned(this))
            return;
        RigidPlayer.Transform = MainTransform.Transform;
    }
}
