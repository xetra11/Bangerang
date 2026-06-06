using System;
using System.IO;
using FlaxEngine;

namespace Game.Game.Player;

public class PlayerSpawnSystem : GamePlugin
{
    public Actor PlayerSpawn;

    public override void Initialize()
    {
        base.Initialize();
        Debug.Log("PlayerSpawnSystem initialized");
    }

    // public override Guid[] GetReferences()
    // {
    //     // Path.Combine():"XHXXJ"
    // }

}
