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

        var rigidBody = PrefabManager.SpawnPrefab(RigidPlayer, spawnTransform);
        NetworkReplicator.SpawnObject(rigidBody);
        NetworkReplicator.SetObjectOwnership(rigidBody, FlaxEngine.Networking.NetworkManager.LocalClientId, NetworkObjectRole.OwnedAuthoritative);
        NetworkReplicator.AddObject(rigidBody);
        rigidBody.FindActor<RigidBody>().AddForce(impulse, ForceMode.Impulse);
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
