using System.Threading.Tasks;
using FlaxEngine;
using FlaxEngine.Networking;

namespace Game.Game.Player;

public class PlayerHitLogic : Script
{
    public Collider Collider;
    public Prefab RigidPlayer;
    public int TimeAfterRegainControl = 100;

    public override void OnStart()
    {
        NetworkReplicator.AddObject(this);
        if (!FlaxEngine.Networking.NetworkManager.IsServer && !FlaxEngine.Networking.NetworkManager.IsHost) return;
        Debug.Logger.Log("PlayerHit Collision registered");
        Collider.CollisionEnter += OnCollision;
    }

    private void OnCollision(Collision collision)
    {
        if (!collision.OtherActor.HasTag("Projectile")) return;
        Debug.Logger.Log("Hit detected");
        Hit(collision.Impulse);
    }

    private void Hit(Vector3 impulse)
    {
        var spawnTransform = Actor.Transform;
        DisableActor(Actor);

        if (RigidPlayer == null)
        {
            Debug.Logger.Log("ERROR: RigidPlayer prefab is not assigned to PlayerHitLogic.");
            return;
        }

        var spawnedActor = PrefabManager.SpawnPrefab(RigidPlayer, spawnTransform);
        if (spawnedActor == null)
        {
            Debug.Logger.Log("ERROR: Failed to spawn RigidPlayer prefab.");
            return;
        }

        NetworkReplicator.SpawnObject(spawnedActor);
        NetworkReplicator.SetObjectOwnership(spawnedActor, FlaxEngine.Networking.NetworkManager.LocalClientId, NetworkObjectRole.OwnedAuthoritative);
        NetworkReplicator.AddObject(spawnedActor);

        var rb = spawnedActor as RigidBody ?? spawnedActor.FindActor<RigidBody>();
        if (rb != null)
        {
            rb.AddForce(impulse, ForceMode.Impulse);
        }
        else
        {
            Debug.Logger.Log("ERROR: Spawned RigidPlayer prefab does not contain a RigidBody actor.");
        }
    }

    private void DisableActor(Actor actor)
    {
        actor.IsActive = false;
        DisableActorOnClient(actor);
    }

    [NetworkRpc( client: true)]
    private void DisableActorOnClient(Actor actor)
    {
        actor.IsActive = false;
    }
}
