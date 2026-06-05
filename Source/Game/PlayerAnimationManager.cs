using FlaxEngine;

namespace Game.Game;

public class PlayerAnimationManager : Script
{
    public AnimatedModel AnimatedModel;

    public override void OnUpdate()
    {
        if (Input.GetMouseButton(MouseButton.Left))
        {
            GameEventSystem.Instance.AddGameEvent(this, ("player", "fire"));
        }
        if (Input.GetKey(KeyboardKeys.W))
        {
             AnimatedModel.SetParameterValue("IsWalking", true);
        }
        AnimatedModel.SetParameterValue("IsWalking", false);
    }
}
