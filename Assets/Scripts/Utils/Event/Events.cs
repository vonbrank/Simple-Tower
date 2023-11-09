using Managers;

namespace Utils.Event
{
    public interface IEvent
    {
    }

    public class StartAttackEvent : IEvent
    {
        public UnitManager.CombatTeam CombatTeam;
    }

    public class UnitAttackFinishEvent : IEvent
    {
        public UnitManager.CombatTeam CombatTeam;
    }

    public class StartCountDownEvent : IEvent
    {
        public int CountDownNumber;
    }
}