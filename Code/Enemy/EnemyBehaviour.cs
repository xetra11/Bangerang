using System;
using Sandbox.Enemy;

namespace Sandbox;

public sealed class EnemyBehaviour : Component
{
  public enum EnemyState
  {
    Idle,
    Patrol,
    Chase,
    Attack
  }

  [Property]
  private EnemyState _currentState = EnemyState.Patrol;
  [Property]
  private Component _activeBehaviourComponent;

  protected override void OnUpdate()
  {
    switch ( _currentState )
    {
      case EnemyState.Idle:
        SetActiveBehaviour( null );
        break;
      case EnemyState.Patrol:
        SetActiveBehaviour( GetOrAddComponent<EnemyPatrol>() );
        break;
      case EnemyState.Chase:
        SetActiveBehaviour( GetOrAddComponent<EnemyChase>() );
        break;
      case EnemyState.Attack:
        SetActiveBehaviour( null );
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
  }

  private void SetActiveBehaviour( Component behaviourComponent )
  {
    if ( _activeBehaviourComponent == behaviourComponent ) return;

    if ( _activeBehaviourComponent != null )
    {
      _activeBehaviourComponent.Enabled = false;
    }

    _activeBehaviourComponent = behaviourComponent;

    if ( _activeBehaviourComponent != null )
    {
      _activeBehaviourComponent.Enabled = true;
    }
  }
}
