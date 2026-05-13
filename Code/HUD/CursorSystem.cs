using Sandbox.Gadget.Development;

namespace Sandbox.HUD;

public class CursorSystem : GameObjectSystem, DeveloperInputSystem.IOpenGadgetDebuggerEvent
{
  private bool _isHidden = true;
  public CursorSystem(Scene scene) : base(scene)
  {
  }

  public void OnGadgetDebuggerToggle()
  {
    _isHidden = !_isHidden;
    Mouse.Visibility = _isHidden ? MouseVisibility.Hidden : MouseVisibility.Visible;
  }
}
