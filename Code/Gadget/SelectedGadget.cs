namespace Sandbox.Gadget;

public class SelectedGadget : Component
{
	[Property] public GameObject GadgetPrefab { get; set; }

	[Button]
	public void Fire()
	{
		var mainCamera = GameObject.Root.GetComponent<Player>().GetMainCamera();
		GadgetPrefab.GetComponent<global::Gadget>().UseGadget( mainCamera.WorldPosition, mainCamera.WorldRotation.Forward);
	}

	protected override void OnUpdate()
	{
	}
}
