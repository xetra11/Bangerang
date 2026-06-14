using FlaxEngine;

namespace Game.Game;

public class GoalLogic: Script
{
    public Collider Collider;

    public override void OnStart()
    {
        Collider.TriggerEnter += OnPass;
    }

    private void OnPass(PhysicsColliderActor collidedActor)
    {
        if (!collidedActor.HasTag("Player")) return;
        GameEventSystem.Instance.Publish(EventFactory.PlayerPassedGoal(collidedActor));
    }
}
