namespace Sandbox;

public sealed class EnemyLineOfSight : Component
{
  [Property] public float ScannerAngle { get; set; } = 45f;
  [Property] public float ScannerDistance { get; set; } = 10f;
  [Property] public int ScannerRayCount { get; set; } = 10;

  private List<GameObject> _detectedObjects = new List<GameObject>();

  protected override void OnUpdate()
  {
    ScanForward();
    if ( SeePlayer( _detectedObjects ) ) GetComponent<EnemyBehaviour>().SetState( EnemyBehaviour.EnemyState.Chase );
  }

  private bool SeePlayer( List<GameObject> detectedObjects )
  {
    for ( var i = 0; i < _detectedObjects.Count; i++ )
    {
      var detectedObject = _detectedObjects[i];
      if ( detectedObject.Tags.Contains( "player" ) )
      {
        return true;
      }
    }

    return false;
  }

  private void ScanForward()
  {
    _detectedObjects.Clear();

    var forward = WorldRotation.Forward;
    var startPos = WorldPosition;

    for ( var i = 0; i < ScannerRayCount; i++ )
    {
      var t = i / (float)(ScannerRayCount - 1);
      var angle = MathX.Lerp( -ScannerAngle / 2f, ScannerAngle / 2f, t );

      var rayRotation = WorldRotation * Rotation.FromYaw( angle );
      var rayDirection = rayRotation.Forward;

      var trace = Scene.Trace.Ray( startPos, startPos + rayDirection * ScannerDistance ).Run();

      if ( trace.Hit && trace.GameObject != null && !_detectedObjects.Contains( trace.GameObject ) )
      {
        _detectedObjects.Add( trace.GameObject );
      }
    }
  }

  protected override void DrawGizmos()
  {
    if ( !Gizmo.IsSelected ) return;

    Gizmo.Draw.Color = Color.Yellow.WithAlpha( 1 );

    var startPos = Vector3.Zero;
    var leftDir = Rotation.FromYaw( -ScannerAngle / 2f ).Forward;
    var rightDir = Rotation.FromYaw( ScannerAngle / 2f ).Forward;

    Gizmo.Draw.Line( startPos, startPos + leftDir * ScannerDistance );
    Gizmo.Draw.Line( startPos, startPos + rightDir * ScannerDistance );
    Gizmo.Draw.Line( startPos + leftDir * ScannerDistance, startPos + rightDir * ScannerDistance );
  }
}
