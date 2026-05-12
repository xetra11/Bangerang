using Sandbox.Gadget;

namespace Sandbox;

public sealed class PlayerGadgetInput : Component, IGadgetSelectedEvent
{
  private Gadget.Gadget _selectedGadget;

  protected override void OnUpdate()
  {
    if ( Input.Released( "attack1" ) )
    {
      var mainCamera = GameObject.Root.GetComponent<Player>()?.GetMainCamera();
      if ( mainCamera != null && _selectedGadget != null )
      {
        _selectedGadget.GetComponent<global::Sandbox.Gadget.Gadget>()?.UseGadget( mainCamera.WorldPosition, mainCamera.WorldRotation.Forward );
      }
    }

  }

  public void OnGadgetSelected( Gadget.Gadget gadget ) => _selectedGadget = gadget;
}
