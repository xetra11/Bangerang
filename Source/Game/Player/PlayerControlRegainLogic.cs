using System;
using System.Threading.Tasks;
using FlaxEngine;
using FlaxEngine.Networking;

namespace Game.Game.Player;

public class PlayerControlRegainLogic : Script
{
    public Actor ControllerActor;
    public Prefab RigidPlayer;
    public int TimeAfterRegainControl = 100;

    private bool _regainingControl;
    private float _regainControlTime;
    private int _regainControlSequence;

    private void RegainControl()
    {
        if (_regainControlTime > 0) return;
        EnableController();
        _regainingControl = false;
        Debug.Log("Player control regained");
    }

    public void EnableController()
    {
        ControllerActor.IsActive = true;
        EnableControllerOnClient();
    }

    [NetworkRpc(client: true)]
    public void EnableControllerOnClient()
    {
        if (FlaxEngine.Networking.NetworkManager.IsHost) return;
        Debug.Log("ControllerActor enabled");
        ControllerActor.IsActive = true;
    }

    public void DisableController()
    {
        ControllerActor.IsActive = false;
        _regainControlTime = TimeAfterRegainControl;
        _regainingControl = true;
        _ = RegainControlAfterDelayAsync(++_regainControlSequence);
        Debug.Log("Actor disabled");
        DisableControllerOnClient();
    }

    private async Task RegainControlAfterDelayAsync(int regainControlSequence)
    {
        if (_regainControlTime > 0)
            await Task.Delay(TimeSpan.FromSeconds(_regainControlTime));

        if (regainControlSequence != _regainControlSequence || !_regainingControl)
            return;

        _regainControlTime = 0;
        RegainControl();
    }

    public void SpawnRigidPlayer(Vector3 impulse)
    {
        if (RigidPlayer == null)
        {
            Debug.Logger.LogError("Player", "RigidPlayer prefab is not assigned.");
            return;
        }

        if (ControllerActor == null)
        {
            Debug.Logger.LogError("Player", "ControllerActor is not assigned.");
            return;
        }

        var controllerTransform = ControllerActor.Transform;

        DisableController();

        var rigidPlayer = PrefabManager.SpawnPrefab(RigidPlayer, controllerTransform);
        if (rigidPlayer == null)
        {
            Debug.Logger.LogError("Player", "Failed to spawn RigidPlayer prefab.");
            return;
        }

        var rigidBody = rigidPlayer as RigidBody ?? rigidPlayer.GetChild<RigidBody>();
        if (rigidBody == null)
        {
            Debug.LogWarning("RigidPlayer prefab requires a RigidBody root or child.");
            return;
        }
        NetworkReplicator.SpawnObject(rigidPlayer);
        rigidBody.AddForce(impulse, ForceMode.Impulse);
    }

    [NetworkRpc(client: true)]
    public void DisableControllerOnClient()
    {
        if (FlaxEngine.Networking.NetworkManager.IsHost) return;
        Debug.Log("Actor disabled");
        ControllerActor.IsActive = false;
    }

    public override void OnDebugDrawSelected()
    {
        DebugDraw.DrawText("Regaining Control in " + _regainControlTime.ToString("0.00"), new Float2(10,10), Color.Red, 12);
        base.OnDebugDrawSelected();
    }
}
