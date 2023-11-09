using System;
using System.Threading.Tasks;
using Attributes;
using UnityEngine;
using Utils;
using Utils.Event;
using Random = UnityEngine.Random;

namespace Managers
{
    public class UnitManager : StaticInstance<UnitManager>
    {
        public enum CombatTeam
        {
            Player,
            Enemy,
        }

        [SerializeField] private Health[] friendlyUnits;
        [SerializeField] private Health[] enemyUnits;

        // public event Action OnPlayerStartAttack;

        private CombatTeam currentInstigator;
        private int currentAttackFinishCount;

        private EventBinding<UnitAttackFinishEvent> unitAttackFinishEventBinding;
        private EventBinding<StartAttackEvent> startAttackEventBinding;

        private void OnEnable()
        {
            startAttackEventBinding = new EventBinding<StartAttackEvent>(HandleAttackStart);
            EventBus<StartAttackEvent>.Register(startAttackEventBinding);

            unitAttackFinishEventBinding = new EventBinding<UnitAttackFinishEvent>(HandleUnitAttackFinish);
            EventBus<UnitAttackFinishEvent>.Register(unitAttackFinishEventBinding);
        }

        private void OnDisable()
        {
            EventBus<StartAttackEvent>.Deregister(startAttackEventBinding);
            EventBus<UnitAttackFinishEvent>.Deregister(unitAttackFinishEventBinding);
        }


        public Health SelectFriendlyUnit()
        {
            return friendlyUnits[Random.Range(0, friendlyUnits.Length)];
        }

        public Health SelectEnemyUnit()
        {
            return enemyUnits[Random.Range(0, enemyUnits.Length)];
        }

        public void HandleAttackStart(StartAttackEvent startAttackEvent)
        {
            currentInstigator = startAttackEvent.CombatTeam;
            currentAttackFinishCount = 0;
        }

        public async void HandleUnitAttackFinish(UnitAttackFinishEvent unitAttackFinishEvent)
        {
            currentAttackFinishCount += 1;

            if (currentInstigator == CombatTeam.Player && friendlyUnits.Length == currentAttackFinishCount)
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                GameStateManager.Instance.ChangeState(GameState.EnemyTurn);
            }
            else if (currentInstigator == CombatTeam.Enemy && enemyUnits.Length == currentAttackFinishCount)
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                GameStateManager.Instance.ChangeState(GameState.HeroTurn);
            }
        }
    }
}