namespace Sandbox.Gadget;

public class SelectedGadget : Component
{
	[Property] public global::Gadget Gadget { get; set; }

	[Button]
	public void Fire()
	{
		Gadget.UseGadget( GameObject.Root.WorldPosition );
	}

	protected override void OnUpdate()
	{
	}
}
