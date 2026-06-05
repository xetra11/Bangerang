using System;
using FlaxEngine;

namespace Game.Game;

public struct GameEventArgs
{
    public string Type { get; set; }
    public string Data { get; set; }

    public GameEventArgs(string type, string data)
    {
        Type = type;
        Data = data;
    }
}

public struct GameEvent
{
    public object Sender { get; set; }
    public GameEventArgs Args { get; set; }

    public GameEvent(object sender, (string, string) args)
    {
        Sender = sender;
        Args = new GameEventArgs(args.Item1, args.Item2);
    }
    public GameEvent(object sender, GameEventArgs args)
    {
        Sender = sender;
        Args = args;
    }
}

public class GameEventSystem : GamePlugin
{
    public event Action<GameEvent> OnGameEvent;

    public Action<GameEvent> GameEventHandler;

    public override void Initialize()
    {
        base.Initialize();
        Debug.Log("GameEventSystem initialized");
    }

    public static GameEventSystem Instance => PluginManager.GetPlugin<GameEventSystem>();


    public void AddGameEvent(GameEvent gameEvent)
    {
        OnGameEvent?.Invoke(gameEvent);
    }
}
