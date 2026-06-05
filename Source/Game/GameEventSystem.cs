using FlaxEngine;

namespace Game.Game;

public class GameEventSystem: Actor
{
    public override void OnBeginPlay()
    {
        Debug.Log("GameEventSystem initialized");
        base.OnBeginPlay();
    }
}
