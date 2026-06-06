using System;
using System.Collections.Generic;
using System.IO;
using FlaxEngine;

namespace Game.Game.Player;

public class PlayerSpawn: Script
{
    public Prefab PlayerPrefab;

    public override void OnStart()
    {
        PrefabManager.SpawnPrefab(PlayerPrefab, Transform);
    }

}
