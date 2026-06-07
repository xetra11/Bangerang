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
            return;

        var ownerClientId = NetworkReplicator.GetObjectOwnerClientId(Actor);
        var role = NetworkReplicator.GetObjectRole(Actor);
        if (ownerClientId == _configuredOwnerClientId && role == _configuredRole)
            return;

        _configuredOwnerClientId = ownerClientId;
        _configuredRole = role;

        var localClientId = FlaxEngine.Networking.NetworkManager.LocalClientId;
        var isLocalPlayer = ownerClientId == localClientId;

        SetScriptEnabled<PlayerFirstPersonLogic>(isLocalPlayer);
        SetScriptEnabled<PlayerInputManager>(isLocalPlayer);
        SetScriptEnabled<PlayerShootLogic>(isLocalPlayer);
        SetScriptEnabled<PlayerAnimationManager>(isLocalPlayer);
    }

    private void SetScriptEnabled<T>(bool enabled) where T : Script
    {
        var script = Actor.GetScript<T>();
        if (script != null)
            script.Enabled = enabled;
    }
}
