namespace Sandbox.Gameplay;

public sealed class GoalPost : Component, Component.ITriggerListener
{
  public void OnTriggerEnter( Collider other )
  {
    if ( other?.GameObject is null ) return;

    Player player = other.GameObject.GetComponentInParent<Player>();
    if ( player is null || !player.IsValid ) return;

    IGoalEnterEvent.Post( x => x.OnGoalEntered( player ) );
  }

  public interface IGoalEnterEvent : ISceneEvent<IGoalEnterEvent>
  {
    void OnGoalEntered( Player player );
  }
}
