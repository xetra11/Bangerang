using FlaxEngine;
using FlaxEngine.Networking;

namespace Game.Game.Player;

public class NetworkedPlayer : Script
{
    private uint _configuredOwnerClientId = uint.MaxValue;
    private NetworkObjectRole _configuredRole = NetworkObjectRole.None;

    public override void OnUpdate()
    {
        TryConfigureOwnership();
    }

    public void TryConfigureOwnership()
    {
        if (!NetworkReplicator.HasObject(Actor))
        {
            return;
        }

        var ownerClientId = NetworkReplicator.GetObjectOwnerClientId(Actor);
        var role = NetworkReplicator.GetObjectRole(Actor);
        if (ownerClientId == _configuredOwnerClientId && role == _configuredRole)
            return;

        _configuredOwnerClientId = ownerClientId;
        _configuredRole = role;

        RegisterNetworkScript<PlayerShootLogic>();

        var isLocalPlayer = NetworkReplicator.IsObjectOwned(Actor);

        SetLayerRecursively(Actor, isLocalPlayer ? 1 : 0);
        SetScriptEnabled<PlayerFirstPersonLogic>(isLocalPlayer);
        SetScriptEnabled<PlayerInputManager>(isLocalPlayer);
        SetScriptEnabled<PlayerAnimationManager>(isLocalPlayer);
        SetScriptEnabled<PlayerShootLogic>(isLocalPlayer);
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
