using FlaxEngine;
using FlaxEngine.Networking;

namespace Game.Game.Player;

public class PlayerShootLogic : Script
{
    public Prefab Projectile;
    public float SpawnOffset = 100.0f;
    public float ProjectileSpeed = 2000.0f;

    public override void OnUpdate()
    {
        if (!NetworkReplicator.HasObject(this) ||
            !NetworkReplicator.IsObjectOwned(this))
            return;

        if (!Input.GetAction("Fire"))
            return;

        var camera = Camera.MainCamera;
        if (camera == null)
        {
            Debug.LogWarning("PlayerShootLogic requires a main camera.");
            return;
        }

        var cameraTransform = camera.Transform;
        var direction = cameraTransform.Forward;
        var spawnPosition = cameraTransform.Translation + direction * SpawnOffset;

        NetworkedSpawnProjectile(
            spawnPosition,
            direction,
            cameraTransform.Orientation
        );
    }

    private void SpawnProjectile(
        Vector3 spawnPosition,
        Vector3 direction,
        Quaternion orientation
    )
    {
        if (Projectile == null)
        {
            Debug.LogWarning("PlayerShootLogic requires a projectile prefab.");
            return;
        }

        direction.Normalize();

        var projectile = PrefabManager.SpawnPrefab(
            Projectile,
            spawnPosition,
            orientation
        );

        var rigidBody = projectile as RigidBody ?? projectile.GetChild<RigidBody>();
        if (rigidBody == null)
        {
            Debug.LogWarning("Projectile prefab requires a RigidBody root or child.");
            return;
        }

        rigidBody.LinearVelocity = direction * ProjectileSpeed;
    }

    [NetworkRpc(Server = true)]
    private void NetworkedSpawnProjectile(
        Vector3 spawnPosition,
        Vector3 direction,
        Quaternion orientation
    )
    {
        SpawnProjectile(spawnPosition, direction, orientation);
    }
}
