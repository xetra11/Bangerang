using System;
using FlaxEngine;

namespace Game.Game;

public struct GameEventArgs
{
    public string Type { get; set; }
    public string Data { get; set; }
}

public class GameEventSystem : GamePlugin
{
    public event Action<object, (string, string)> OnGameEvent;
    public Action<object, GameEventArgs> GameEventHandler;

    public override void Initialize()
    {
        base.Initialize();
        Debug.Log("GameEventSystem initialized");
    }

    public static GameEventSystem Instance => PluginManager.GetPlugin<GameEventSystem>();

    public void AddGameEvent(object sender, (string type, string data) args)
    {
        OnGameEvent?.Invoke(sender, args);
    }
}
