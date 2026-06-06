using FlaxEngine;

namespace Game.Game.Player;

public class PlayerShootLogic : Script
{
    public override void OnUpdate()
    {
        if (Input.GetAction("Fire"))
        {
            Debug.Log("Player fired");
        }
    }
}
