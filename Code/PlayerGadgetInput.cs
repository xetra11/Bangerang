using Sandbox.Gadget;

namespace Sandbox;

public sealed class PlayerGadgetInput : Component
{
  [Property] public GameObject gadget { get; set; }

  protected override void OnUpdate()
  {
    if ( Input.Released( "attack1" ) )
    {

      var mainCamera = GameObject.Root.GetComponent<Player>()?.GetMainCamera();
      if ( mainCamera != null && gadget != null )
      {
        gadget.GetComponent<global::Sandbox.Gadget.Gadget>()?.UseGadget( mainCamera.WorldPosition, mainCamera.WorldRotation.Forward );
      }
    }
  }
}
