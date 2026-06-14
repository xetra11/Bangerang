namespace Game.Game;

public class EventFactory
{
    public static GameEvent GreedyCollectEvent(object sender) => new(sender, (EventType.GreedyCollect, 1));
    public static GameEvent PlayerPassedGoal(object sender) => new(sender, (EventType.PlayerGoalPass, null));
    // public static GameEvent PlayerMoveEvent(object sender) => new(sender, ("player_move", "forwards"));
}
