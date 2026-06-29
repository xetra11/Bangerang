using FlaxEngine;
using FlaxEngine.Networking;

namespace Game.Game.Player;

public class Player : Script
{

    public override void OnUpdate()
    {
        if (NetworkReplicator.HasObject(Actor))
        {
            DetermineOwnership();
        }
    }

    private void DetermineOwnership()
    {
        RegisterNetworkScript<PlayerShootLogic>();
        RegisterNetworkScript<PlayerControlRegainLogic>();

        var isLocalOwner = NetworkReplicator.IsObjectOwned(Actor);
        SetScriptEnabled<PlayerFirstPersonLogic>(isLocalOwner);
        SetScriptEnabled<PlayerInputManager>(isLocalOwner);
        SetScriptEnabled<PlayerAnimationManager>(isLocalOwner);
        SetScriptEnabled<PlayerShootLogic>(isLocalOwner);

        if (!isLocalOwner)
        {
            var audioListener = Actor.GetChild<AudioListener>();
            if (audioListener != null) Destroy(audioListener);
        }

        SetLayerRecursively(Actor, isLocalOwner ? 1 : 0);
    }

    private static void SetLayerRecursively(Actor actor, int layer)
    {
        actor.Layer = layer;
        foreach (var child in actor.Children)
            SetLayerRecursively(child, layer);
    }

    private void RegisterNetworkScript<T>() where T : Script
    {
        var script = Actor.GetScript<T>();
        if (script != null && !NetworkReplicator.HasObject(script))
            NetworkReplicator.AddObject(script, Actor);
    }

    private void SetScriptEnabled<T>(bool enabled) where T : Script
    {
        var script = Actor.GetScript<T>();
        if (script != null)
            script.Enabled = enabled;
    }
}
