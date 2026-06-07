using System.Threading.Tasks;
using FlaxEngine;
using FlaxEngine.Networking;

namespace Game.Game.Greedy;

public class NetworkedGreedy : Script
{
    public override void OnStart()
    {
        NetworkReplicator.AddObject(Actor);
    }
}
