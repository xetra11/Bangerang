namespace Sandbox.Gadget.Development;

public class DeveloperInputSystem : GameObjectSystem
{
  public DeveloperInputSystem( Scene scene ) : base( scene )
  {
    Listen( Stage.FinishUpdate, 10, CheckInput, "DebuggerInputCheckInput" );
  }

  private void CheckInput()
  {
    if ( Input.Pressed( "GadgetsDebug" ) )
    {
      Log.Info( "Gadget Debugger Toggled" );
      IOpenGadgetDebuggerEvent.Post( x => x.OnGadgetDebuggerToggle() );
    }
  }

  public interface IOpenGadgetDebuggerEvent : ISceneEvent<IOpenGadgetDebuggerEvent>
  {
    public void OnGadgetDebuggerToggle();
  }
}
