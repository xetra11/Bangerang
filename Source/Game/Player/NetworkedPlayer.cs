using FlaxEngine;
using FlaxEngine.Networking;

namespace Game.Game.Player;

public class NetworkedPlayer : Script
{
    public void OnSpawned()
    {
        Debug.Log($"HasObject: {NetworkReplicator.HasObject(Actor)}");
        Debug.Log($"Role: {NetworkReplicator.GetObjectRole(Actor)}");
        Debug.Log($"Owner: {NetworkReplicator.GetObjectOwnerClientId(Actor)}");
        Debug.Log($"Owned locally: {NetworkReplicator.IsObjectOwned(Actor)}");
        Debug.Log($"Simulated locally: {NetworkReplicator.IsObjectSimulated(Actor)}");
        Debug.Log($"Replicated locally: {NetworkReplicator.IsObjectReplicated(Actor)}");

        if (!NetworkReplicator.IsObjectOwned(Actor))
        {
            Actor.GetScript<CharacterController>().IsActive = false;
            Actor.GetScript<PlayerFirstPersonLogic>().Enabled = false;
        }
    }
}
