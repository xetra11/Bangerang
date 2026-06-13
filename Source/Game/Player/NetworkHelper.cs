using FlaxEngine;
using FlaxEngine.Networking;

namespace Game.Game.Player;

public static class NetworkHelper
{
    public static bool IsLocalOwner( Object obj)
    {
        var ownerClientId = NetworkReplicator.GetObjectOwnerClientId(obj);
        var localClientId = FlaxEngine.Networking.NetworkManager.LocalClientId;
        return ownerClientId == localClientId;
    }
}
