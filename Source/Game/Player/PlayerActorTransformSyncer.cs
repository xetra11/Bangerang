using FlaxEngine;

namespace Game.Game.Player;

public class PlayerActorTransformSyncer: Script
{
    public Actor MainTransform;
    public Actor RigidPlayer;

    public override void OnUpdate()
    {
        RigidPlayer.Transform = MainTransform.Transform;
    }
}
