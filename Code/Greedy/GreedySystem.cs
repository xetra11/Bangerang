namespace Sandbox.Greedy;

public sealed class GreedySystem : GameObjectSystem<GreedySystem>
{
  private static int GreedyAmount { get; set; }

  public GreedySystem( Scene scene ) : base( scene )
  {
    GreedyAmount = 0;
  }

  public int GetGreedyAmount()
  {
    return GreedyAmount;
  }

  public void Collect()
  {
    GreedyAmount++;
    IGreedyCollectEvent.Post( x => x.OnGreedyCollect(GreedyAmount) );
  }
}

public interface IGreedyCollectEvent : ISceneEvent<IGreedyCollectEvent>
{
  void OnGreedyCollect( int current);
}
