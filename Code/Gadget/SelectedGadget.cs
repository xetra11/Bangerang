namespace Sandbox.Gadget;

public class SelectedGadget : Component
{
	[Property] public GameObject GadgetPrefab { get; set; }

	[Button]
	public void Fire()
	{
		GadgetPrefab.GetComponent<global::Gadget>().UseGadget( GameObject.Root.WorldPosition );
	}

	protected override void OnUpdate()
	{
	}
}
