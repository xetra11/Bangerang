namespace Sandbox.Gameplay;

public class WinConditionSystem : GameObjectSystem<WinConditionSystem>, GoalPost.IGoalEnterEvent
{
  public WinConditionSystem( Scene scene ) : base( scene )
  {
  }

  public void OnGoalEntered( Player player )
  {
    Scene.LoadFromFile( "scenes/greedyscore.scene" );
  }
}
