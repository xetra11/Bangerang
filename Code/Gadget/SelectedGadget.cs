namespace Sandbox.Gadget;

public class SelectedGadget : Component
{
	[Property] public List<GameObject> GadgetPrefabs { get; set; } = new();
	[Property] public GameObject GadgetPrefab { get; set; }

	private int selectedGadgetIndex = 0;

	[Button]
	public void Fire()
	{
		var mainCamera = GameObject.Root.GetComponent<Player>()?.GetMainCamera();
		if ( mainCamera != null && GadgetPrefab != null )
		{
			GadgetPrefab.GetComponent<global::Sandbox.Gadget.Gadget>()?.UseGadget( mainCamera.WorldPosition, mainCamera.WorldRotation.Forward );
		}
	}

	protected override void OnUpdate()
	{

	if ( Input.Pressed( "Slot1" ) || Input.Pressed( "1" ) )
	{
		selectedGadgetIndex = 0;
		UpdateSelectedGadget();
	}
	else if ( Input.Pressed( "Slot2" ) || Input.Pressed( "2" ) )
	{
		selectedGadgetIndex = 1;
		UpdateSelectedGadget();
	}
	else if ( Input.Pressed( "Slot3" ) || Input.Pressed( "3" ) )
	{
		selectedGadgetIndex = 2;
		UpdateSelectedGadget();
	}
}

private void UpdateSelectedGadget()
{
	if ( GadgetPrefabs != null && selectedGadgetIndex >= 0 && selectedGadgetIndex < GadgetPrefabs.Count )
	{
		GadgetPrefab = GadgetPrefabs[selectedGadgetIndex];
	}
	}
}
