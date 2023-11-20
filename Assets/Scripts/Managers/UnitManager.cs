using System;
using System.Linq;
using Attributes;
using Cysharp.Threading.Tasks;
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

        private CombatTeamInfo[] combatTeamInfoList;


        private EventBinding<StartAttackEvent> startAttackEventBinding;

        protected override void Awake()
        {
            base.Awake();


            var allUnit = FindObjectsOfType<UnitBase>();

            combatTeamInfoList = new[]
            {
                new CombatTeamInfo
                {
                    CombatTeam = CombatTeam.Player,
                    UnitList = allUnit.Where(item => item.CombatTeam == CombatTeam.Player).ToArray()
                },
                new CombatTeamInfo
                {
                    CombatTeam = CombatTeam.Enemy,
                    UnitList = allUnit.Where(item => item.CombatTeam == CombatTeam.Enemy).ToArray()
                },
            };
        }

        private void OnEnable()
        {
        }

        private void OnDisable()
        {
            EventBus<StartAttackEvent>.Deregister(startAttackEventBinding);
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

        public void UnitDied(UnitBase unitBase)
        {
            for (int i = 0; i < combatTeamInfoList.Length; i++)
            {
                ref var combatTeamInfo = ref combatTeamInfoList[i];
                if (combatTeamInfo.CombatTeam == unitBase.CombatTeam)
                {
                    combatTeamInfo.UnitList =
                        combatTeamInfo.UnitList.Where(item => item != unitBase).ToArray();
                    Debug.Log(
                        $"Team {unitBase.CombatTeam} has one unit died. Rest units num = {combatTeamInfo.UnitList.Length}");

                    if (combatTeamInfo.UnitList.Length == 0)
                    {
                        switch (combatTeamInfo.CombatTeam)
                        {
                            case CombatTeam.Player:
                                GameStateManager.Instance.ChangeState(GameState.Lose);
                                break;
                            case CombatTeam.Enemy:
                                GameStateManager.Instance.ChangeState(GameState.Win);
                                break;
                        }
                    }

                    break;
                }
            }
        }

        public CombatTeamInfo? GetCombatTeam(CombatTeam combatTeam)
        {
            for (int i = 0; i < combatTeamInfoList.Length; i++)
            {
                if (combatTeamInfoList[i].CombatTeam == combatTeam)
                {
                    return combatTeamInfoList[i];
                }
            }

            return null;
        }
    }
}