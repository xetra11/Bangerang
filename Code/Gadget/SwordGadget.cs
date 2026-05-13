using System;

namespace Sandbox.Gadget;

public sealed class SwordGadget : global::Sandbox.Gadget.Gadget
{
  [Property] public String Name { get; set; }
  [Property] public GameObject ProjectilePrefab { get; set; }

  public override void UseGadget( Transform gadgetOrigin, Vector3 playerFacingDirection )
  {
    var projectileInstance = ProjectilePrefab.Clone( gadgetOrigin );
    projectileInstance.GetComponent<Projectile>().Direction = playerFacingDirection;
    Log.Info( $"Spawned projectile at {gadgetOrigin}" );
  }

  protected override void OnUpdate()
  {
  }
}
