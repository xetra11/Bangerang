using System.Threading.Tasks;
using FlaxEngine;

namespace Game.Game.Greedy;

public class GreedyPickupLogic: Script
{
    public Collider Collider;
    public AudioClip Clip;

    public override void OnStart()
    {
        Collider.TriggerEnter += OnPickup;
    }

    private void OnPickup(PhysicsColliderActor collidedActor)
    {
        if (!collidedActor.HasTag("Player")) return;
        AudioSystem.Instance.PlayAudio(Transform, Clip);
        GameEventSystem.Instance.Publish(EventFactory.GreedyCollectEvent(collidedActor));
        Destroy(Actor);
    }

}
