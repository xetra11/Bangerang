using FlaxEngine;
using FlaxEngine.GUI;
using FlaxEngine.Networking;

namespace Game.Game.Goal;

public class GoalUIManager : Script
{

    public ControlReference<Panel> EndGamePanel;

    public override void OnStart()
    {
        EndGamePanel.Control.Visible = false;
        GameEventSystem.Instance.OnGameEvent += @event =>
        {
            if (@event.Sender is Actor actor)
            {
                if (!NetworkReplicator.IsObjectOwned(actor)) return;
            }

            if (@event.Args.Type == EventType.PlayerGoalPass)
            {
                EndGamePanel.Control.Visible = true;
                Debug.Log("Player won game");
            }
        };
    }

}
