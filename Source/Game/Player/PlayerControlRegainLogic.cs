using FlaxEngine;
using FlaxEngine.Networking;

namespace Game.Game.Player;

public class PlayerControlRegainLogic : Script
{
    public Actor ControllerActor;
    public Actor RigidBodyActor;
    public int TimeAfterRegainControl = 100;

    private bool _regainingControl;
    private float _regainControlTime;


    public override void OnUpdate()
    {
        if (_regainingControl)
        {
            Debug.Log("Regain Loop");
            _regainControlTime -= Time.DeltaTime;
            RegainControl();
        }
    }

    private void RegainControl()
    {
        if (_regainControlTime > 0) return;
        EnableActor();
        _regainingControl = false;
        Debug.Log("Player control regained");
    }

    public void EnableActor()
    {
        ControllerActor.Transform = RigidBodyActor.Transform;
        ControllerActor.IsActive = true;
        RigidBodyActor.IsActive = false;
        EnableActorOnClient();
    }

    [NetworkRpc(client: true)]
    public void EnableActorOnClient()
    {
        if (FlaxEngine.Networking.NetworkManager.IsHost) return;
        Debug.Log("ControllerActor enabled");
        ControllerActor.IsActive = true;
        RigidBodyActor.IsActive = false;
    }

    public void DisableActor()
    {
        RigidBodyActor.Transform = ControllerActor.Transform;
        ControllerActor.IsActive = false;
        RigidBodyActor.IsActive = true;
        _regainControlTime = TimeAfterRegainControl;
        _regainingControl = true;
        DisableActorOnClient();
    }

    [NetworkRpc(client: true)]
    public void DisableActorOnClient()
    {
        if (FlaxEngine.Networking.NetworkManager.IsHost) return;
        Debug.Log("Actor disabled");
        ControllerActor.IsActive = false;
        RigidBodyActor.IsActive = true;
    }

    public override void OnDebugDrawSelected()
    {
        DebugDraw.DrawText("Regaining Control in " + _regainControlTime.ToString("0.00"), new Float2(10,10), Color.Red, 12);
        base.OnDebugDrawSelected();
    }
}
