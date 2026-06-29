using System.Threading.Tasks;
using FlaxEngine;
using FlaxEngine.Networking;

namespace Game.Game.Player;

public class PlayerHitLogic : Script
{
    public Collider Collider;
    private bool _hasCollided = false;

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
        if (_hasCollided) return;
        Debug.Logger.Log("Collision detected");
        _hasCollided = true;
        // Defer spawning out of the physics simulation step; handled in OnUpdate.
        _pendingImpulse = collision.Impulse.Negative;
        _hitPending = true;
    }

    private void Hit(Vector3 impulse)
    {
        Debug.Logger.Log("Resolving Hit");

        // if (RigidPlayer == null)
        // {
        //     Debug.Logger.LogError("Player","RigidPlayer prefab is not assigned to PlayerHitLogic.");
        //     return;
        // }
        //
        // Actor.GetScript< PlayerControlRegainLogic>().DisableActor();
        // var rigidBody = RigidPlayer.GetScript<RigidBody>();
        // if (rigidBody != null)
        // {
        //     rigidBody.AddForce(impulse, ForceMode.Impulse);
        // }

        _hasCollided = false;
    }


}
