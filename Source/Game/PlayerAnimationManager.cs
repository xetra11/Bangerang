using FlaxEngine;

namespace Game.Game;

public class PlayerAnimationManager : Script
{
    public AnimatedModel AnimatedModel;

    public override void OnUpdate()
    {
        if (Input.GetKey(KeyboardKeys.W))
        {
            AnimatedModel.SetParameterValue("IsWalking", true);
        }
        else
        {
            AnimatedModel.SetParameterValue("IsWalking", false);
        }

    }
}
