using FlaxEngine;

namespace Game.Game;

public class PlayerAnimationManager : Script
{
    public AnimatedModel AnimatedModel;

    public override void OnUpdate()
    {
        AnimatedModel.SetParameterValue("IsWalking", Input.GetAction("NavigateUp"));
    }
}
