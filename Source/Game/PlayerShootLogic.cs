using System;
using FlaxEngine;

namespace Game.Game;

public class PlayerShootLogic : Script
{
    public override void OnStart()
    {
        GameEvents.OnGameEvent += (sender, args) =>
        {
            if (args is { Item1: "player", Item2: "fire" })
            {
                Debug.Log("Player fired, moving forward");
            }
        };
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }
}
