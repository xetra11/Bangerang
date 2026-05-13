using Sandbox.Gadget;

namespace Sandbox;

public sealed class PlayerGadgetInput : Component, IGadgetSelectedEvent
{
  private Gadget.Gadget _selectedGadget;

  protected override void OnUpdate()
  {
    if ( Input.Released( "attack1" ) )
    {
      var player = GameObject.Root.GetComponent<Player>();
      if ( player == null )
      {
        Log.Error( "Player not defined for Gadget Input" );
        return;
      }

      var mainCamera = player.GetMainCamera();
      var gadgetMount = player.GetComponent<GadgetMount>();

      if ( gadgetMount == null )
      {
        Log.Error( "Missing Gadget Mount on Player" );
        return;
      }

      if ( mainCamera != null && _selectedGadget != null )
      {
        _selectedGadget.GetComponent<global::Sandbox.Gadget.Gadget>()?.UseGadget( gadgetMount.GetMountPosition(), mainCamera.WorldRotation.Forward );
      }
    }
  }

  public void OnGadgetSelected( int index, Gadget.Gadget gadget ) => _selectedGadget = gadget;
}
