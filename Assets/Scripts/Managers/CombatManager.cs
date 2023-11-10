using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;
using Utils.Event;

namespace Managers
{
    public class CombatManager : StaticInstance<CombatManager>
    {
        public struct CombatInfo
        {
            public bool PlayerCanAttack;
        }

        public static event Action<CombatInfo> OnCombatInfoUpdate;


        private bool playerCanAttack;

        private EventBinding<StartAttackEvent> startAttackEventBinding;

        private void OnEnable()
        {
            GameStateManager.OnAfterStateChanged += OnAfterGameStateChanged;
            InputManager.Instance.PlayerInputActions.Player.Attack.performed += HandlePlayerInputAttack;

            startAttackEventBinding = new EventBinding<StartAttackEvent>(HandleStartAttackEvent);
            EventBus<StartAttackEvent>.Register(startAttackEventBinding);
        }

        private void OnDisable()
        {
            GameStateManager.OnAfterStateChanged -= OnAfterGameStateChanged;
            InputManager.Instance.PlayerInputActions.Player.Attack.performed -= HandlePlayerInputAttack;

            EventBus<StartAttackEvent>.Deregister(startAttackEventBinding);
        }

        private void HandleStartAttackEvent(StartAttackEvent startAttackEvent)
        {
            if (startAttackEvent.CombatTeam == UnitManager.CombatTeam.Player)
            {
                playerCanAttack = false;
                OnCombatInfoUpdate?.Invoke(new CombatInfo
                {
                    PlayerCanAttack = playerCanAttack
                });
            }
        }

        void OnAfterGameStateChanged(GameState newGameState)
        {
            if (newGameState == GameState.HeroTurn)
            {
                playerCanAttack = true;
                OnCombatInfoUpdate?.Invoke(new CombatInfo
                {
                    PlayerCanAttack = playerCanAttack
                });
            }
        }

        void HandlePlayerInputAttack(InputAction.CallbackContext context)
        {
            if (playerCanAttack)
            {
                EventBus<StartAttackEvent>.Raise(new StartAttackEvent
                {
                    CombatTeam = UnitManager.CombatTeam.Player
                });
            }
        }
    }
}