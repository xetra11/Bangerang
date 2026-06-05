using System;
using FlaxEngine;

namespace Game.Game;

public class PlayerShootLogic : Script
{
    public override void OnStart()
    {
        GameEventSystem.Instance.OnGameEvent += @event =>
        {
            if (@event.Args.Type == "player_action" && @event.Args.Type == "fire")
            {
                Debug.Log("Player fired");
            }
        };
    }
}
