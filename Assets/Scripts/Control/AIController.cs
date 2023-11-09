using System;
using Managers;
using UnityEngine;
using Utils.Event;

namespace Control
{
    public class AIController : ControllerBase
    {
        private void OnEnable()
        {
            GameStateManager.OnAfterStateChanged += OnGameStateChange;
        }

        private void OnDisable()
        {
            GameStateManager.OnAfterStateChanged -= OnGameStateChange;
        }

        private void OnGameStateChange(GameState newGameState)
        {
            if (newGameState == GameState.EnemyTurn)
            {
                fighter.Attack(UnitManager.Instance.SelectFriendlyUnit(), UnitManager.CombatTeam.Enemy);
            }
        }

        protected override void HandleStartAttack(StartAttackEvent startAttackEvent)
        {
            base.HandleStartAttack(startAttackEvent);
            if (startAttackEvent.CombatTeam != UnitManager.CombatTeam.Enemy)
            {
                return;
            }

            fighter.Attack(UnitManager.Instance.SelectFriendlyUnit(), UnitManager.CombatTeam.Enemy);
        }
    }
}