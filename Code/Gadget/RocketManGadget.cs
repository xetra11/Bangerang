using System;

namespace Sandbox.Gadget;

public sealed class RocketManGadget : Gadget
{

  public override void UseGadget( Transform gadgetOrigin, Player player )
  {
  }

  public override string GadgetName() => "Rocket Man";

  protected override void OnUpdate()
  {
  }
}
