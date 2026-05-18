using System;

namespace Sandbox.Enemy;

public class EnemyMovement: Component
{
  [Property, Range( 0.1f, 1 )] private readonly float _speed = 0.1f;
  [Property]
  public Vector3 DestinationTarget { get; set; }

  protected override void OnUpdate()
  {
    var direction = (DestinationTarget - WorldPosition).Normal;
    if (direction.Length > 0.001f)
    {
      var targetRotation = Rotation.LookAt(direction);
      WorldRotation = Rotation.Slerp(WorldRotation, targetRotation, Time.Delta * 5f);
    }

    var distance = MathF.Min( (_speed * 100) * Time.Delta, WorldPosition.Distance( DestinationTarget ) );
    WorldPosition += direction * distance;
  }
}
