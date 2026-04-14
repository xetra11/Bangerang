namespace Sandbox.Gadget;

public class Projectile: Component
{
	[Property] public float ThrowSpeed { get; set; } = 1000f;
	[Property] public float Gravity { get; set; } = 800f;

	private Vector3 _velocity;

	protected override void OnStart()
	{
		_velocity = WorldRotation.Forward * ThrowSpeed;
	}

	protected override void OnUpdate()
	{
		if ( WorldPosition.z < -10 ) DestroyGameObject();
		_velocity += Vector3.Down * Gravity * Time.Delta;
		WorldPosition += _velocity * Time.Delta;
	}
}
