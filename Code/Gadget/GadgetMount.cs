namespace Sandbox;

public sealed class GadgetMount : Component
{
  [Property] public GameObject GadgetMountPos { get; set; }

  public Transform GetMountPosition()
  {
    return GadgetMountPos.WorldTransform;
  }
}
