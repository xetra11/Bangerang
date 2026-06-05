using FlaxEngine;

namespace Game.Game;

public class ExitOnEsc : Script
{
    public override void OnUpdate()
    {
        if (Input.GetKeyUp(KeyboardKeys.Escape))
            Engine.RequestExit();
    }
}