using System;
using FlaxEngine;
using FlaxEngine.Networking;

namespace Game.Game.Player;

public class RagdollFirstPersonLogic : Script
{
    public Actor CameraAnchor;
    public Camera Camera;
    public float CameraSmoothing = 20.0f;
    private float _pitch;

    public override void OnStart()
    {

    }

    public override void OnUpdate()
    {
        if (NetworkReplicator.GetObjectOwnerClientId(Actor) != FlaxEngine.Networking.NetworkManager.LocalClientId)
        {
            return;
        }

        Screen.CursorVisible = false;
        Screen.CursorLock = CursorLockMode.Locked;

        var mouseDelta = new Float2(
            Input.GetAxis("Mouse X"),
            Input.GetAxis("Mouse Y")
        );

        _pitch = Mathf.Clamp(_pitch + mouseDelta.Y, -88.0f, 88.0f);
    }

    public override void OnFixedUpdate()
    {
        float factor = Mathf.Saturate(CameraSmoothing * Time.DeltaTime);

        UpdateCamera(factor);
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
}
