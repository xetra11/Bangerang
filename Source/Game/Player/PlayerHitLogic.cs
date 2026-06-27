using System.Threading.Tasks;
using FlaxEngine;
using FlaxEngine.Networking;

namespace Game.Game.Player;

public class PlayerHitLogic : Script
{
    public Collider Collider;
    public Actor RigidPlayer;
    public int TimeAfterRegainControl = 100;

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
        if (RigidPlayer == null)
        {
            Debug.Logger.LogError("Player","RigidPlayer prefab is not assigned to PlayerHitLogic.");
            return;
        }

        DisableActor();
        var rigidBody = RigidPlayer.GetScript<RigidBody>();
        if (rigidBody != null)
        {
            rigidBody.AddForce(impulse, ForceMode.Impulse);
        }
        _ = RegainControl();
    }


    private async Task RegainControl()
    {
        await Task.Delay(TimeAfterRegainControl);
        EnableActor();
    }

    private void EnableActor()
    {
        Actor.Transform = RigidPlayer.Transform;
        Actor.IsActive = true;
        RigidPlayer.IsActive = false;
        EnableActorOnClient();
    }

    [NetworkRpc(client: true)]
    private void EnableActorOnClient()
    {
        if (FlaxEngine.Networking.NetworkManager.IsHost) return;
        Debug.Log("Actor enabled");
        Actor.IsActive = true;
        RigidPlayer.IsActive = false;
    }

    private void DisableActor()
    {
        RigidPlayer.Transform = Actor.Transform;
        Actor.IsActive = false;
        RigidPlayer.IsActive = true;
        DisableActorOnClient();
    }

    [NetworkRpc(client: true)]
    private void DisableActorOnClient()
    {
        if (FlaxEngine.Networking.NetworkManager.IsHost) return;
        Debug.Log("Actor disabled");
        Actor.IsActive = false;
        RigidPlayer.IsActive = true;
    }
}
