using System;

namespace Sandbox.Gadget;

public sealed class SwordGadget : global::Sandbox.Gadget.Gadget
{
  [Property] public GameObject ProjectilePrefab { get; set; }

  public override void UseGadget( Transform gadgetOrigin, Vector3 playerFacingDirection )
  {
    var projectileInstance = ProjectilePrefab.Clone( gadgetOrigin );
    if ( projectileInstance == null )
    {
      Log.Error( "Failed to clone projectile" );
      return;
    }
    projectileInstance.GetComponent<Projectile>().Direction = playerFacingDirection;
    Log.Info( $"Spawned projectile at {gadgetOrigin}" );
  }

  public override string GadgetName() => "Sword";

}
