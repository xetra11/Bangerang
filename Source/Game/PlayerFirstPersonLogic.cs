using System;
using FlaxEngine;

namespace Game.Game;

public class PlayerFirstPersonLogic : Script
{
    public CharacterController PlayerController;
    public Actor CameraAnchor;
    public Camera Camera;

    public float CameraSmoothing = 20.0f;

    public bool CanJump = true;
    public bool UseMouse = true;
    public float JumpForce = 800;

    public float Friction = 8.0f;
    public float GroundAccelerate = 5000;
    public float AirAccelerate = 10000;
    public float MaxVelocityGround = 400;
    public float MaxVelocityAir = 200;

    private Vector3 _velocity;
    private bool _jump;

    private float _pitch;
    private float _yaw;

    private float _horizontal;
    private float _vertical;

    public override void OnUpdate()
    {
        if (UseMouse)
        {
            Screen.CursorVisible = false;
            Screen.CursorLock = CursorLockMode.Locked;

            var mouseDelta = new Float2(
                Input.GetAxis("Mouse X"),
                Input.GetAxis("Mouse Y")
            );

            _pitch = Mathf.Clamp(_pitch + mouseDelta.Y, -88.0f, 88.0f);
            _yaw += mouseDelta.X;
        }

        if (CanJump && Input.GetAction("Jump"))
            _jump = true;
    }

    public override void OnFixedUpdate()
    {
        float factor = Mathf.Saturate(CameraSmoothing * Time.DeltaTime);

        UpdateCamera(factor);
        UpdatePlayerRotation(factor);

        Vector3 wishDir = GetMovementDirection();

        Vector3 newVelocity;

        if (PlayerController.IsGrounded)
        {
            newVelocity = MoveGround(wishDir, Horizontal(_velocity));
            newVelocity.Y = -Mathf.Abs(Physics.Gravity.Y * 0.5f);
        }
        else
        {
            newVelocity = MoveAir(wishDir, Horizontal(_velocity));
            newVelocity.Y = _velocity.Y;
        }

        if (newVelocity.Length < 0.05f)
            newVelocity = Vector3.Zero;

        if (_jump && PlayerController.IsGrounded)
            newVelocity.Y = JumpForce;

        _jump = false;

        newVelocity.Y += -Mathf.Abs(Physics.Gravity.Y * 2.5f) * Time.DeltaTime;

        if ((PlayerController.Flags & CharacterController.CollisionFlags.Above) != 0)
        {
            if (newVelocity.Y > 0)
                newVelocity.Y = 0;
        }

        PlayerController.Move(newVelocity * Time.DeltaTime);
        _velocity = newVelocity;
    }

    private void UpdateCamera(float factor)
    {
        var targetOrientation = Quaternion.Euler(_pitch, 0, 0);

        CameraAnchor.LocalOrientation = Quaternion.Lerp(
            CameraAnchor.LocalOrientation,
            targetOrientation,
            factor
        );

        var camTransform = Camera.Transform;

        camTransform.Translation = Vector3.Lerp(
            camTransform.Translation,
            CameraAnchor.Position,
            factor
        );

        camTransform.Orientation = CameraAnchor.Orientation;

        Camera.Transform = camTransform;
    }

    private void UpdatePlayerRotation(float factor)
    {
        var targetPlayerRotation = Quaternion.Euler(0, _yaw, 0);

        PlayerController.Orientation = Quaternion.Lerp(
            PlayerController.Orientation,
            targetPlayerRotation,
            factor
        );
    }

    private Vector3 GetMovementDirection()
    {
        float inputH = Input.GetAxis("Horizontal") + _horizontal;
        float inputV = Input.GetAxis("Vertical") + _vertical;

        _horizontal = 0;
        _vertical = 0;

        var direction = new Vector3(inputH, 0.0f, inputV);

        if (direction.LengthSquared < 0.001f)
            return Vector3.Zero;

        direction.Normalize();

        // Important:
        // Movement direction is based on raw yaw, not CameraAnchor.LocalEulerAngles.
        // This avoids mixing smoothed camera rotation with gameplay input.
        direction = Vector3.Transform(
            direction,
            Quaternion.Euler(0, _yaw, 0)
        );

        return direction;
    }

    private Vector3 Horizontal(Vector3 v)
    {
        return new Vector3(v.X, 0, v.Z);
    }

    private Vector3 Accelerate(
        Vector3 accelDir,
        Vector3 prevVelocity,
        float accelerate,
        float maxVelocity
    )
    {
        if (accelDir.LengthSquared < 0.001f)
            return prevVelocity;

        float projVel = Vector3.Dot(prevVelocity, accelDir);
        float accelVel = accelerate * Time.DeltaTime;

        if (projVel + accelVel > maxVelocity)
            accelVel = maxVelocity - projVel;

        return prevVelocity + accelDir * accelVel;
    }

    private Vector3 MoveGround(Vector3 accelDir, Vector3 prevVelocity)
    {
        float speed = prevVelocity.Length;

        if (Math.Abs(speed) > 0.01f)
        {
            float drop = speed * Friction * Time.DeltaTime;
            prevVelocity *= Mathf.Max(speed - drop, 0) / speed;
        }

        return Accelerate(
            accelDir,
            prevVelocity,
            GroundAccelerate,
            MaxVelocityGround
        );
    }

    private Vector3 MoveAir(Vector3 accelDir, Vector3 prevVelocity)
    {
        return Accelerate(
            accelDir,
            prevVelocity,
            AirAccelerate,
            MaxVelocityAir
        );
    }

    public override void OnDebugDraw()
    {
        var trans = PlayerController.Transform;

        DebugDraw.DrawWireCapsule(
            trans.Translation,
            trans.Orientation * Quaternion.Euler(90, 0, 0),
            PlayerController.Radius,
            PlayerController.Height,
            Color.Blue
        );
    }
}
