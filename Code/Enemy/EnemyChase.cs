using System;
using System.Collections.Generic;

namespace Sandbox.Enemy;

public sealed class EnemyChase : Component
{
  [Property] public GameObject Target;

  protected override void OnUpdate()
  {
    if ( Target == null ) return;
    GetComponent<EnemyMovement>().DestinationTarget = Target.WorldPosition;
  }

}
