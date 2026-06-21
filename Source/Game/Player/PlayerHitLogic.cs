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

    // Pending hit captured during a collision callback (in-physics-simulation) and
    // processed later in OnUpdate. Spawning a RigidBody inside the simulation step crashes.
    private bool _hitPending;
    private Vector3 _pendingImpulse;

    public override void OnStart()
    {
        NetworkReplicator.AddObject(this);
        if (!FlaxEngine.Networking.NetworkManager.IsServer && !FlaxEngine.Networking.NetworkManager.IsHost) return;
        Debug.Logger.Log("PlayerHit Collision registered");
        Collider.CollisionEnter += OnCollision;
    }

    public override void OnFixedUpdate()
    {
        if (!_hitPending) return;
        _hitPending = false;
        Hit(_pendingImpulse);
    }

    private void OnCollision(Collision collision)
    {
        if (!collision.OtherActor.HasTag("Projectile")) return;
        Debug.Logger.Log("Hit detected");
        // Defer spawning out of the physics simulation step; handled in OnUpdate.
        _pendingImpulse = collision.Impulse.Negative;
        _hitPending = true;
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
        AddCameraToRagdoll();

        if (_spawnedRigidBodyActor == null)
        {
            Debug.Logger.LogError("Player","Failed to spawn RigidPlayer prefab.");
            return;
        }

        NetworkReplicator.SpawnObject(_spawnedRigidBodyActor);
        NetworkReplicator.SetObjectOwnership(_spawnedRigidBodyActor, FlaxEngine.Networking.NetworkManager.LocalClientId, NetworkObjectRole.OwnedAuthoritative);

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

    private void AddCameraToRagdoll()
    {
        AddCameraToRagdollOnClient();

        if (FlaxEngine.Networking.NetworkManager.LocalClientId == NetworkReplicator.GetObjectOwnerClientId(Actor))
        {
            var ragdollFirstPersonLogic = _spawnedRigidBodyActor.AddScript<RagdollFirstPersonLogic>();
            ragdollFirstPersonLogic.CameraAnchor = _spawnedRigidBodyActor.FindActor<CameraAnchor>();
            ragdollFirstPersonLogic.Camera = Actor.FindScript<PlayerFirstPersonLogic>().Camera;
        }

    }

    [NetworkRpc( client: true)]
    private void AddCameraToRagdollOnClient()
    {
        if (_spawnedRigidBodyActor == null)
        {
            Debug.LogError("AddCameraToRagdollOnClient: _spawnedRigidBodyActor is null.");
            return;
        }

        var ragdollFirstPersonLogic = _spawnedRigidBodyActor.AddScript<RagdollFirstPersonLogic>();
        ragdollFirstPersonLogic.CameraAnchor = _spawnedRigidBodyActor.FindActor<CameraAnchor>();
        ragdollFirstPersonLogic.Camera = Actor.FindScript<PlayerFirstPersonLogic>().Camera;
    }

    private async Task RegainControl()
    {
        await Task.Delay(TimeAfterRegainControl);
        Debug.Log("RegainControl called");
        Actor.Transform = _spawnedRigidBodyActor.Transform;
        Actor.EulerAngles = Vector3.Zero;
        EnableActor(Actor);
        NetworkReplicator.DespawnObject(_spawnedRigidBodyActor);
        NetworkReplicator.RemoveObject(_spawnedRigidBodyActor);
        Destroy(_spawnedRigidBodyActor);
    }

    private void EnableActor(Actor actor)
    {
        actor.IsActive = true;
        EnableActorOnClient();
    }

    [NetworkRpc( client: true)]
    private void EnableActorOnClient()
    {
        Actor.IsActive = true;
    }

    private void DisableActor(Actor actor)
    {
        actor.IsActive = false;
        DisableActorOnClient();
    }

    [NetworkRpc( client: true)]
    private void DisableActorOnClient()
    {
        Actor.IsActive = false;
    }
}
