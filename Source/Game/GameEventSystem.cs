using System;
using FlaxEngine;

namespace Game.Game;

public enum EventType
{
    GreedyCollect,
    PlayerGoalPass,
    PlayerMove,
}

public struct GameEventArgs
{
    public EventType Type { get; set; }
    public object Data { get; set; }

    public GameEventArgs(EventType type, object data)
    {
        Type = type;
        Data = data;
    }
}

public struct GameEvent
{
    public object Sender { get; set; }
    public GameEventArgs Args { get; set; }

    public GameEvent(object sender, (EventType, object) args)
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

    public override void Deinitialize()
    {
        OnGameEvent = null;
        base.Deinitialize();
    }

    public static GameEventSystem Instance => PluginManager.GetPlugin<GameEventSystem>();


    public void Publish(GameEvent gameEvent)
    {
        OnGameEvent?.Invoke(gameEvent);
    }
}
