public sealed class Player : Component
{
  private CameraComponent _mainCamera;
  private SpawnPoint _spawnPoint;

  protected override void OnStart()
  {
    _spawnPoint ??= Scene.GetAllComponents<SpawnPoint>().FirstOrDefault();
    if ( _spawnPoint == null ) Log.Error( "SpawnPoint not found yet" );

    Mouse.Visibility = MouseVisibility.Hidden;
    _mainCamera = Scene.GetAllComponents<CameraComponent>().FirstOrDefault();

    if ( _mainCamera == null )
    {
      Log.Warning( "No camera found in the scene." );
    }
  }

  public CameraComponent GetMainCamera() => _mainCamera;

  protected override void OnUpdate()
  {
    if ( WorldPosition.z < -1000 )
    {
      if ( Network.IsOwner )
      {
        WorldPosition = _spawnPoint.WorldPosition;
      }
      else
      {
        DestroyGameObject();
      }
    }
  }
}
