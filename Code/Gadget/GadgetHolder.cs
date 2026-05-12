namespace Sandbox.Gadget;

public class GadgetHolder : Component
{
  [Property] public List<GameObject> Gadgets { get; set; } = new();

}
