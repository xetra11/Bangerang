namespace Sandbox;

public sealed class AI_PatrolPathNode : Component
{
	protected override void OnUpdate()
	{
	}

  protected override void DrawGizmos()
  {
    Gizmo.Draw.Color = Color.Cyan;
    Gizmo.Draw.LineSphere( Vector3.Zero, 20f );
  }
}
