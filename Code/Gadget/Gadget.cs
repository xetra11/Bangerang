namespace Sandbox.Gadget;

public abstract class Gadget : Component
{
  public abstract void UseGadget( Vector3 playerLocation, Vector3 playerFacingDirection );
}
