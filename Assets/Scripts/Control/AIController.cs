using System;
using Managers;
using Unit;
using UnityEngine;
using Utils.Event;

namespace Control
{
    public class AIController : ControllerBase
    {
        // protected override void OnEnable()
        // {
        //     base.OnEnable();
        //     GameStateManager.OnAfterStateChanged += OnGameStateChange;
        // }
        //
        // protected override void OnDisable()
        // {
        //     base.OnDisable();
        //     GameStateManager.OnAfterStateChanged -= OnGameStateChange;
        // }

        private void OnGameStateChange(GameState newGameState)
        {
            // if (newGameState == GameState.EnemyTurn)
            // {
            //     fighter.Attack(UnitManager.Instance.SelectFriendlyUnit(), UnitManager.CombatTeam.Enemy);
            // }
        }
    }
}