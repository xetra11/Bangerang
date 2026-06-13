using System.Threading.Tasks;
using FlaxEngine;

namespace Game.Game.Greedy;

public class GreedyPickupLogic: Script
{
    public Collider Collider;
    public AudioClip Clip;
    private bool _collided;

    public override void OnUpdate()
    {
        Collider.TriggerEnter += OnPickup;
    }

    private void OnPickup(PhysicsColliderActor collidedActor)
    {
        if (_collided) return;
        if (!collidedActor.HasTag("Player")) return;
        _collided = true;
        AudioSystem.Instance.PlayAudio(Transform, Clip);
        GameEventSystem.Instance.Publish(EventFactory.GreedyCollectEvent(collidedActor));
        Destroy(Actor);
    }

}
