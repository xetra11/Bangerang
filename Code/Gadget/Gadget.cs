namespace Sandbox.Gadget;

public abstract class Gadget : Component
{
  public virtual void UseGadget( Transform gadgetOrigin, Vector3 playerFacingDirection ){}
  public virtual void UseGadget( Transform gadgetOrigin, Player player ) {}
  public abstract string GadgetName();
}
