using System;
using FlaxEngine;

namespace Game.Game;

public struct GameEventArgs
{
    public string Type { get; set; }
    public string Data { get; set; }
}

public static class GameEvents {
    public static Action<object, GameEventArgs> GameEventHandler;
    public static event Action<object, (string, string)> OnGameEvent;

    public static void AddGameEvent(object sender, (string type, string data) args)
    {
        OnGameEvent?.Invoke(sender, args);
    }
}


public class GameEventSystem: Actor
{

    public override void OnBeginPlay()
    {
        base.OnBeginPlay();
        Debug.Log("GameEventSystem initialized");
    }


}
