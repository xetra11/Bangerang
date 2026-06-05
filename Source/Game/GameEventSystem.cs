using System;
using FlaxEngine;

namespace Game.Game;

public struct GameEventArgs
{
    public string Type { get; set; }
    public string Data { get; set; }
}

public class GameEvents
{
    private static GameEvents _instance;
    public Action<object, GameEventArgs> GameEventHandler;
    public event Action<object, (string, string)> OnGameEvent;

    public static void Init()
    {
        _instance = new GameEvents();
    }

    public static GameEvents Instance()
    {
        if (_instance == null) throw new InvalidOperationException("GameEvents instance is null, please initialize with GameEvents.Init()");
        return _instance;
    }

    public void AddGameEvent(object sender, (string type, string data) args)
    {
        OnGameEvent?.Invoke(sender, args);
    }
}

public class GameEventSystem : Script
{
    public override void OnAwake()
    {
        base.OnEnable();
        GameEvents.Init();
        Debug.Log("GameEventSystem initialized");
    }

}
