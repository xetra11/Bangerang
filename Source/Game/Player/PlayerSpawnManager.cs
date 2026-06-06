using System;
using System.Collections.Generic;
using System.IO;
using FlaxEngine;
using FlaxEngine.Networking;

namespace Game.Game.Player;

public class PlayerSpawn: Script
{
    public Prefab PlayerPrefab;

    public override void OnStart()
    {
        var player = PrefabManager.SpawnPrefab(PlayerPrefab, Transform);
        NetworkReplicator.SpawnObject(player);
    }

}
