using System;

namespace Sandbox.Gadget;

public sealed class StoneGadget : global::Gadget
{
  [Property] public String Name { get; set; }
  [Property] public GameObject ProjectilePrefab { get; set; }

  public override void UseGadget( Vector3 playerLocation, Vector3 playerFacingDirection )
  {
    var projectileInstance = ProjectilePrefab.Clone( playerLocation );
    projectileInstance.GetComponent<Projectile>().Direction = playerFacingDirection;
    Log.Info( $"Spawned projectile at {playerLocation}" );
  }

  protected override void OnUpdate()
  {
  }
}
