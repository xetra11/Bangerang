using FlaxEngine;

namespace Game.Game.Player;

public class PlayerAnimationManager : Script
{
    public AnimatedModel AnimatedModel;

    public override void OnUpdate()
    {
        AnimatedModel.SetParameterValue("IsWalking", Input.GetAction("NavigateUp"));
    }
}
