using System;
using Managers;
using UnityEngine;
using Utils.Event;

namespace Control
{
    public class PlayerController : ControllerBase
    {
        protected override void HandleStartAttack(StartAttackEvent startAttackEvent)
        {
            base.HandleStartAttack(startAttackEvent);
            if (startAttackEvent.CombatTeam != UnitManager.CombatTeam.Player)
            {
                return;
            }

            fighter.Attack(UnitManager.Instance.SelectEnemyUnit(), UnitManager.CombatTeam.Player);
        }
    }
}