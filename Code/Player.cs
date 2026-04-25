
public sealed class Player : Component
{
	private CameraComponent _mainCamera;

	protected override void OnStart()
	{
		_mainCamera = Scene.GetAllComponents<CameraComponent>().FirstOrDefault();

		if ( _mainCamera == null )
		{
			Log.Warning( "No camera found in the scene." );
		}

	}

	public CameraComponent GetMainCamera() => _mainCamera;

	protected override void OnUpdate()
	{
	}
}
