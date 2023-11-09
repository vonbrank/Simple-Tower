using System;
using System.Linq;
using System.Threading.Tasks;
using Attributes;
using Unit;
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

        [Serializable]
        public struct CombatTeamInfo
        {
            public CombatTeam CombatTeam;
            public UnitBase[] UnitList;
        }

        // [SerializeField] private Health[] friendlyUnits;
        // [SerializeField] private Health[] enemyUnits;

        [SerializeField] private CombatTeamInfo[] combatTeamInfoList;

        // public event Action OnPlayerStartAttack;

        private CombatTeam currentInstigator;
        private int currentAttackFinishCount;
        private CombatTeamInfo currentCombatTeamInfo;

        private EventBinding<UnitAttackFinishEvent> unitAttackFinishEventBinding;
        private EventBinding<StartAttackEvent> startAttackEventBinding;

        protected override void Awake()
        {
            base.Awake();
            foreach (var combatTeamInfo in combatTeamInfoList)
            {
                foreach (var unitBase in combatTeamInfo.UnitList)
                {
                    unitBase.CombatTeam = combatTeamInfo.CombatTeam;
                }
            }
        }

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

        public bool SelectEnemyUnit(CombatTeam combatTeam, out UnitBase unit)
        {
            var enemyCombatTeam = combatTeam switch
            {
                CombatTeam.Player => CombatTeam.Enemy,
                CombatTeam.Enemy => CombatTeam.Player,
                _ => CombatTeam.Enemy
            };

            for (int i = 0; i < combatTeamInfoList.Length; i++)
            {
                if (combatTeamInfoList[i].CombatTeam == enemyCombatTeam)
                {
                    unit = combatTeamInfoList[i].UnitList[Random.Range(0, combatTeamInfoList[i].UnitList.Length)];
                    return true;
                }
            }

            unit = null;
            return false;
        }

        private void HandleAttackStart(StartAttackEvent startAttackEvent)
        {
            currentInstigator = startAttackEvent.CombatTeam;
            currentAttackFinishCount = 0;
            bool teamInfoExisted = false;
            for (int i = 0; i < combatTeamInfoList.Length; i++)
            {
                if (combatTeamInfoList[i].CombatTeam == startAttackEvent.CombatTeam)
                {
                    currentCombatTeamInfo = combatTeamInfoList[i];
                    teamInfoExisted = true;
                }
            }

            if (!teamInfoExisted)
            {
                FinishAttack();
            }
        }

        private void HandleUnitAttackFinish(UnitAttackFinishEvent unitAttackFinishEvent)
        {
            currentAttackFinishCount += 1;

            // Debug.Log($"Count =  {currentAttackFinishCount} Length = {currentCombatTeamInfo.UnitList.Length}");

            if (currentAttackFinishCount == currentCombatTeamInfo.UnitList.Length)
            {
                FinishAttack();
            }
        }

        private async void FinishAttack()
        {
            if (currentInstigator == CombatTeam.Player)
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                GameStateManager.Instance.ChangeState(GameState.EnemyTurn);
            }
            else if (currentInstigator == CombatTeam.Enemy)
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                GameStateManager.Instance.ChangeState(GameState.HeroTurn);
            }
        }

        public void UnitDied(UnitBase unitBase)
        {
            for (int i = 0; i < combatTeamInfoList.Length; i++)
            {
                ref var combatTeamInfo = ref combatTeamInfoList[i];
                if (combatTeamInfo.CombatTeam == unitBase.CombatTeam)
                {
                    combatTeamInfo.UnitList =
                        combatTeamInfo.UnitList.Where(item => item != unitBase).ToArray();
                    // Debug.Log($"team {unitBase.CombatTeam} num = {combatTeamInfo.UnitList.Length}");
                    break;
                }
            }
        }
    }
}