using System.Threading.Tasks;
using FlaxEngine;
using FlaxEngine.Networking;

namespace Game.Game.Player;

public class PlayerHitLogic : Script
{
    public Collider Collider;
    public Prefab RigidPlayer;
    public int TimeAfterRegainControl = 100;

    private Actor _spawnedRigidBodyActor;
    private Transform _groundedPosition;

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
        Hit(collision.Impulse.Negative);
    }

    private void Hit(Vector3 impulse)
    {
        var spawnTransform = Actor.Transform;
        DisableActor(Actor);

        if (RigidPlayer == null)
        {
            Debug.Logger.LogError("Player","RigidPlayer prefab is not assigned to PlayerHitLogic.");
            return;
        }

        _spawnedRigidBodyActor = PrefabManager.SpawnPrefab(RigidPlayer, spawnTransform);
        if (_spawnedRigidBodyActor == null)
        {
            Debug.Logger.LogError("Player","Failed to spawn RigidPlayer prefab.");
            return;
        }

        NetworkReplicator.SpawnObject(_spawnedRigidBodyActor);
        NetworkReplicator.SetObjectOwnership(_spawnedRigidBodyActor, FlaxEngine.Networking.NetworkManager.LocalClientId, NetworkObjectRole.OwnedAuthoritative);
        NetworkReplicator.AddObject(_spawnedRigidBodyActor);

        var activeRigidBody = _spawnedRigidBodyActor as RigidBody ?? _spawnedRigidBodyActor.FindActor<RigidBody>();
        if (activeRigidBody != null)
        {
            activeRigidBody.AddForce(impulse, ForceMode.Impulse);
            _ = RegainControl();
        }
        else
        {
            Debug.Logger.LogError("Player", "Spawned RigidPlayer prefab does not contain a RigidBody actor.");
        }
    }

    private async Task RegainControl()
    {
        await Task.Delay(TimeAfterRegainControl);
        _groundedPosition = _spawnedRigidBodyActor.Transform;
        Debug.Log("RegainControl called");
        EnableActor(Actor);
        NetworkReplicator.DespawnObject(_spawnedRigidBodyActor);
        NetworkReplicator.RemoveObject(_spawnedRigidBodyActor);
        Destroy(_spawnedRigidBodyActor);
    }

    private void EnableActor(Actor actor)
    {
        actor.Transform = _groundedPosition;
        actor.IsActive = true;
        EnableActorOnClient(actor);
    }

    [NetworkRpc( client: true)]
    private void EnableActorOnClient(Actor actor)
    {
        actor.IsActive = true;
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
