using FlaxEngine;
using FlaxEngine.Networking;

namespace Game.Game.Goal;

public class GoalUIManager : Script
{

    public override void OnStart()
    {
        GameEventSystem.Instance.OnGameEvent += @event =>
        {
            if (@event.Sender is Actor actor)
            {
                if (!NetworkReplicator.IsObjectOwned(actor)) return;
            }

            if (@event.Args.Type == EventType.PlayerGoalPass)
            {
                Debug.Log("Player won game");
            }
        };
    }

}
