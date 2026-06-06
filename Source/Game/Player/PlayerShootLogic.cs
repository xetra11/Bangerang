using FlaxEngine;

namespace Game.Game.Player;

public class PlayerShootLogic : Script
{
    public override void OnUpdate()
    {
        if (Input.GetAction("Fire"))
        {
            var pointLight = new PointLight();
            pointLight.Color = Color.Red;
            pointLight.Parent = Scene;

            var audioSource = new AudioSource();
            // audioSource.Transform = location;
            // audioSource.Clip = clip;
            audioSource.Parent = Scene;
            Debug.Log("Player fired");
        }
    }
}
