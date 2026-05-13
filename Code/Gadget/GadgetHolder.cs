namespace Sandbox.Gadget;

public class GadgetHolder : Component
{
  [Property] public List<GameObject> Gadgets { private get; set; } = new();

  public List<Gadget> GetGadgets()
  {
    return Gadgets.Select( x => x.GetComponent<Gadget>() ).ToList();
  }
}
