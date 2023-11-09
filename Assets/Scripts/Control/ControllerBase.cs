using System;
using Combat;
using Managers;
using Unit;
using UnityEngine;
using Utils.Event;

namespace Control
{
    [RequireComponent(typeof(Fighter))]
    public class ControllerBase : MonoBehaviour
    {
        protected EventBinding<StartAttackEvent> startAttackEventBinding;
        protected UnitBase unitBase;

        protected virtual void OnEnable()
        {
            startAttackEventBinding = new EventBinding<StartAttackEvent>(HandleStartAttack);
            EventBus<StartAttackEvent>.Register(startAttackEventBinding);
            // Debug.Log("fuck");
        }

        protected virtual void OnDisable()
        {
            EventBus<StartAttackEvent>.Deregister(startAttackEventBinding);
            // Debug.Log("fuck");
        }

        private void Awake()
        {
            unitBase = GetComponent<UnitBase>();
        }

        protected virtual void HandleStartAttack(StartAttackEvent startAttackEvent)
        {
            if (startAttackEvent.CombatTeam != unitBase.CombatTeam)
            {
                return;
            }

            if (UnitManager.Instance.SelectEnemyUnit(unitBase.CombatTeam, out UnitBase enemy))
            {
                unitBase.Fighter.Attack(enemy);
            }
            else
            {
            }
        }
    }
}