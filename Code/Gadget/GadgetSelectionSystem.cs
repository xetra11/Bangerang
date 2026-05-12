namespace Sandbox.Gadget;

public class GadgetSelectionSystem : GameObjectSystem
{
  private GadgetHolder _gadgetHolder;

  public GadgetSelectionSystem( Scene scene ) : base( scene )
  {
    Listen( Stage.FinishUpdate, 10, CheckSelection, "CheckGadgetSelection" );
  }

  private void CheckSelection()
  {
    _gadgetHolder ??= Scene.GetAllComponents<GadgetHolder>().FirstOrDefault();

    if ( _gadgetHolder == null )
    {
      Log.Error( "GadgetHolder not found yet" );
      return;
    }

    if ( _gadgetHolder.Gadgets.Count == 0 )
    {
      Log.Error( "GadgetHolder is empty" );
      return;
    }


    if ( Input.Released( "Slot1" ) )
      SelectGadget( 0 );

    if ( Input.Released( "Slot2" ) )
      SelectGadget( 1 );

    if ( Input.Released( "Slot3" ) )
      SelectGadget( 2 );
  }

  private void SelectGadget( int index )
  {
    if ( index >= _gadgetHolder.Gadgets.Count )
    {
      Log.Error( $"Invalid index: {index}" );
      return;
    }

    var gadget = _gadgetHolder.Gadgets[index].GetComponent<Gadget>();
    Log.Info( $"Selected gadget: {gadget}" );
    if ( gadget == null )
      return;

    IGadgetSelectedEvent.Post( x => x.OnGadgetSelected( gadget ) );
  }
}

public interface IGadgetSelectedEvent : ISceneEvent<IGadgetSelectedEvent>
{
  void OnGadgetSelected( Gadget gadget );
}
