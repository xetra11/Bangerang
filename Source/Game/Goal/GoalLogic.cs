using FlaxEngine;

namespace Game.Game;

public class GoalLogic: Script
{
    public Collider Collider;
    private bool _collided;

    public override void OnUpdate()
    {
        Collider.TriggerEnter += OnPass;
    }

    private void OnPass(PhysicsColliderActor collidedActor)
    {
        if (_collided) return;
        if (!collidedActor.HasTag("Player")) return;
        _collided = true;
        GameEventSystem.Instance.Publish(EventFactory.PlayerPassedGoal(collidedActor));
    }
}
