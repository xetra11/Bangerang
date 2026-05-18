using System;

namespace Sandbox.Enemy;

public sealed class EnemyPatrol : Component
{
  [Property] private readonly List<AI_PatrolPathNode> _pathNodes = [];

  private AI_PatrolPathNode _currentTargetNode = null;

  protected override void OnUpdate()
  {
    if ( _currentTargetNode == null ) DetermineNextMovementTarget();
    if ( _currentTargetNode == null ) return;

    if ( WorldPosition.Distance( _currentTargetNode.WorldPosition ) < 10f ) DetermineNextMovementTarget();
    GetComponent<EnemyMovement>().DestinationTarget = _currentTargetNode.WorldPosition;
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

    var currentIndex = _pathNodes.IndexOf( _currentTargetNode );
    // 2 + 1 % 3 = 0
    // 0 + 1 % 3 = 1
    // 1 + 1 % 3 = 2
    var nextIndex = (currentIndex + 1) % _pathNodes.Count;

    _currentTargetNode = _pathNodes[nextIndex];
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
