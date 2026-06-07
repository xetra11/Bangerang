using FlaxEngine;

namespace Game.Game.Player;

public class PlayerShootLogic : Script
{
    public Prefab Projectile;
    public float SpawnOffset = 100.0f;
    public float ProjectileSpeed = 2000.0f;

    public override void OnUpdate()
    {
        if (!Input.GetAction("Fire"))
            return;

        var camera = Camera.MainCamera;
        if (Projectile == null || camera == null)
        {
            Debug.LogWarning("PlayerShootLogic requires a projectile prefab and main camera.");
            return;
        }

        var cameraTransform = camera.Transform;
        var direction = cameraTransform.Forward;
        var spawnPosition = cameraTransform.Translation + direction * SpawnOffset;
        var projectile = PrefabManager.SpawnPrefab(
            Projectile,
            spawnPosition,
            cameraTransform.Orientation
        );

        var rigidBody = projectile as RigidBody ?? projectile.GetChild<RigidBody>();
        if (rigidBody == null)
        {
            Debug.LogWarning("Projectile prefab requires a RigidBody root or child.");
            return;
        }

        rigidBody.LinearVelocity = direction * ProjectileSpeed;
    }
}
