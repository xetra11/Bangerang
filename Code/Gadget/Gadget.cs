namespace Sandbox.Gadget;

public abstract class Gadget : Component
{
  public abstract void UseGadget( Transform gadgetOrigin, Vector3 playerFacingDirection );
}
