using System;

namespace Sandbox.Gadget;

public sealed class RocketManGadget : Gadget
{
  public CameraComponent Camera { get; set; }
  private float _speed = 1000f;

  public override string GadgetName() => "Rocket Man";

  public override void UseGadget( Transform gadgetOrigin, Player player )
  {
    player.GetOrAddComponent<RocketManGadget>().Camera = player.GetMainCamera();
  }

  protected override void OnUpdate()
  {
    WorldPosition += Camera.WorldRotation.Forward * _speed * Time.Delta;
  }
}
