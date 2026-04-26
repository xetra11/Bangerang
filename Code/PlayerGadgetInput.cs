using Sandbox.Gadget;

namespace Sandbox;

public sealed class PlayerGadgetInput : Component
{
  [Property] public SelectedGadget SelectedGadget { get; set; }

  protected override void OnUpdate()
  {
    if ( Input.Released( "attack1" ) )
    {
      SelectedGadget.Fire();
    }
  }
}
