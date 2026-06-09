using FlaxEngine;
using FlaxEngine.Networking;

namespace Game.Game;

public class Projectile : Script
{
    private const float Lifetime = 2.0f;
    private float _remainingLifetime;

    public override void OnStart()
    {
        _remainingLifetime = Lifetime;
        NetworkReplicator.SpawnObject(Actor);
    }

    public override void OnUpdate()
    {
        _remainingLifetime -= Time.DeltaTime;
        if (_remainingLifetime <= 0.0f)
        {
            NetworkReplicator.DespawnObject(Actor);
            Destroy(Actor);
        }
    }
}
