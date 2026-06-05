using FlaxEngine;

namespace Game.Game;

public class PlayerInputManager : Script
{
    public override void OnUpdate()
    {
        if (Input.GetMouseButton(MouseButton.Left))
        {
            GameEventSystem.Instance.AddGameEvent(EventFactory.PlayerFireEvent(this));
        }


    }
}
