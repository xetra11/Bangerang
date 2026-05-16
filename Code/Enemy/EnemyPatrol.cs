namespace Sandbox.Enemy;

public sealed class EnemyPatrol : Component
{
  [Property] private readonly List<AI_PatrolPathNode> _pathNodes = [];
  [Property, Range( 0.1f, 1 )] private readonly float _speed = 0.1f;

  private AI_PatrolPathNode _currentTargetNode = null;

  protected override void OnUpdate()
  {
    if ( _currentTargetNode == null ) DetermineNextMovementTarget();

    if ( WorldPosition.Normal.SubtractDirection( _currentTargetNode.WorldPosition, 1.0f ).IsNearlyZero( 0.1f ) )
    {
      DetermineNextMovementTarget();
    }

    WorldPosition = Vector3.Lerp( WorldPosition, _currentTargetNode.WorldPosition, Time.Delta * _speed );
  }


  private void DetermineNextMovementTarget()
  {
    Log.Info( "Determining next movement target" );
    if ( _pathNodes.Count == 0 )
    {
      Log.Warning( "No path nodes available, returning current position" );
      return;
    }

    if ( _currentTargetNode == null )
    {
      // Set first node if empty
      _currentTargetNode = _pathNodes[0];
      Log.Info( $"Setting first node: {_currentTargetNode.GameObject.Name}" );
      return;
    }

    for ( var i = 0; i < _pathNodes.Count; i++ )
    {
      var nextNode = _pathNodes[i];
      if ( nextNode == _currentTargetNode )
      {
        Log.Info( $"Skipping current node: {_currentTargetNode.GameObject.Name}" );
        continue;
      }

      // When reached the last node
      if ( i == _pathNodes.Count - 1 )
      {
        // Set next node to first node (to reset path loop)
        _currentTargetNode = _pathNodes[0];
        // but still return position or last node (otherwise it's skipped)
        Log.Info( $"Reached last node, resetting to first node: {_currentTargetNode?.GameObject.Name}" );
        return;
      }

      Log.Info( $"Setting next node {nextNode.GameObject.Name} as current node" );
      _currentTargetNode = nextNode;
    }
  }

  protected override void DrawGizmos()
  {
    var text = _currentTargetNode?.GameObject.Name;
    Gizmo.Draw.Text( text, WorldTransform.ToLocal( WorldTransform ) );
    DrawNodePaths();
  }

  private void DrawNodePaths()
  {
    AI_PatrolPathNode currentStartNode = null;
    Gizmo.Draw.Color = Color.Cyan;
    Gizmo.Draw.LineThickness = 0.5f;

    if ( _pathNodes.Count == 1 )
    {
      Gizmo.Draw.Line( Vector3.Zero, WorldTransform.PointToLocal( _pathNodes[0].WorldPosition ) );
    }

    for ( var index = 0; index < _pathNodes.Count; index++ )
    {
      var nextNode = _pathNodes[index];
      if ( currentStartNode == null )
      {
        Gizmo.Draw.Line( Vector3.Zero, WorldTransform.PointToLocal( nextNode.WorldPosition ) );
      }
      else
      {
        Gizmo.Draw.Line( WorldTransform.PointToLocal( currentStartNode.WorldPosition ), WorldTransform.PointToLocal( nextNode.WorldPosition ) );
      }

      currentStartNode = nextNode;
      if ( index == _pathNodes.Count - 1 )
      {
        Gizmo.Draw.Line( WorldTransform.PointToLocal( currentStartNode.WorldPosition ), WorldTransform.PointToLocal( _pathNodes[0].WorldPosition ) );
      }
    }
  }
}
