using System.Threading.Tasks;
using FlaxEngine;

namespace Game.Game.Player;

public class PlayerHitLogic : Script
{
    public Collider Collider;
    public int TimeAfterRegainControl = 100;

    public override void OnStart()
    {
        if (!FlaxEngine.Networking.NetworkManager.IsServer && !FlaxEngine.Networking.NetworkManager.IsHost) return;
        Debug.Logger.Log("PlayerHit Collision registered");
        Collider.CollisionEnter += OnCollision;
    }

    private void OnCollision(Collision collision)
    {
        if (!collision.OtherActor.HasTag("Projectile")) return;
        Debug.Logger.Log("Hit detected");
        Hit(collision.Impulse);
    }

    private void Hit(Vector3 impulse)
    {
        var characterController = Actor.FindActor<CharacterController>();
        if (characterController == null)
        {
            Debug.LogError("Character controller not found");
            return;
        }

        characterController.IsActive = false;
        Debug.Logger.Log("Adding force to rigid body {}", impulse);
        // rigidBody.AddForce(new Vector3(0, 1, 0), ForceMode.Impulse);
        // await Task.Delay(TimeAfterRegainControl);
        // characterController.IsActive = false;
    }
}
