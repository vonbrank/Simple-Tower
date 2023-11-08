using System;
using Managers;
using UnityEngine;

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
                fighter.Attack(UnitManager.Instance.SelectFriendlyUnit());
            }
        }
    }
}