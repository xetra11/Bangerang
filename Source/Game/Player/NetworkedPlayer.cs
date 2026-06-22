using FlaxEngine;
using FlaxEngine.Networking;

namespace Game.Game.Player;

public class NetworkedPlayer : Script
{
    public Actor PlayerController;
    private uint _configuredOwnerClientId = uint.MaxValue;
    private NetworkObjectRole _configuredRole = NetworkObjectRole.None;

    public override void OnUpdate()
    {
        TryConfigureOwnership();
    }

    public void TryConfigureOwnership()
    {
        if (!NetworkReplicator.HasObject(PlayerController))
        {
            return;
        }

        var ownerClientId = NetworkReplicator.GetObjectOwnerClientId(PlayerController);
        var role = NetworkReplicator.GetObjectRole(PlayerController);
        if (ownerClientId == _configuredOwnerClientId && role == _configuredRole)
            return;

        _configuredOwnerClientId = ownerClientId;
        _configuredRole = role;

        RegisterNetworkScript<PlayerShootLogic>();

        var isLocalPlayer = NetworkReplicator.IsObjectOwned(PlayerController);

        SetLayerRecursively(PlayerController, isLocalPlayer ? 1 : 0);
        var audioListener = PlayerController.GetChild<AudioListener>();
        if (audioListener != null) Destroy(audioListener);

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
        var script = PlayerController.GetScript<T>();
        if (script != null && !NetworkReplicator.HasObject(script))
            NetworkReplicator.AddObject(script, PlayerController);
    }

    private void SetScriptEnabled<T>(bool enabled) where T : Script
    {
        var script = PlayerController.GetScript<T>();
        if (script != null)
            script.Enabled = enabled;
    }
}
