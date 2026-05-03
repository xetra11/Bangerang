using System.Dynamic;
using Sandbox;
using Sandbox.Diagnostics;

public sealed class Greedy : Component, Component.ITriggerListener
{
  [Property] public int GreedValue { get; set; }
  [Property] public SoundEvent CoinSound { get; set; }

  protected override void OnUpdate()
  {
  }

  public void OnTriggerEnter( Collider other )
  {
    if ( other?.GameObject is null ) return;

    Player player = other.GameObject.GetComponentInParent<Player>();
    if ( player is null || !player.IsValid ) return;

    if ( CoinSound != null ) Sound.Play( CoinSound, WorldPosition );
    IGreedyCollectEvent.Post(x => x.OnGreedyCollect());
    DestroyGameObject();
  }

}

public interface IGreedyCollectEvent : ISceneEvent<IGreedyCollectEvent>
{
  void OnGreedyCollect();
}

public interface IGreedyPickup
{
	void OnPickup( int amount );
}
