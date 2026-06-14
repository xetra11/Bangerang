using FlaxEngine;
using FlaxEngine.GUI;
using FlaxEngine.Networking;
using Game.Game.Player;

namespace Game.Game.Greedy;

public class GreedyUIManager : Script
{
    private int _greedyCount;

    public ControlReference<Label> GreedyAmountLabel;

    public override void OnStart()
    {
        GameEventSystem.Instance.OnGameEvent += @event =>
        {
            if (@event.Sender is Actor actor)
            {
                if (!NetworkReplicator.IsObjectOwned(actor)) return;
            }
            if (Validate()) return;

            if (@event.Args.Type == EventType.GreedyCollect)
            {
                Debug.Log("Add greedy to UI");
                _greedyCount++;

                GreedyAmountLabel.Control.Text.Value = _greedyCount.ToString();
            }
        };
    }

    private bool Validate()
    {
        if (GreedyAmountLabel == null)
        {
            Debug.LogError("GreedyAmountLabel is null");
            return true;
        }

        if (GreedyAmountLabel.Control == null)
        {
            Debug.LogError("GreedyAmountLabel Control is null");
            return true;
        }

        return false;
    }
}
