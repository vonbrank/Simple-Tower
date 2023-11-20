using System;
using Cysharp.Threading.Tasks;
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
        private UnitManager.CombatTeam currentInstigator;
        private int currentAttackFinishCount;

        private EventBinding<StartAttackEvent> startAttackEventBinding;
        private EventBinding<UnitAttackFinishEvent> unitAttackFinishEventBinding;

        private void OnEnable()
        {
            GameStateManager.OnAfterStateChanged += OnAfterGameStateChanged;
            InputManager.Instance.PlayerInputActions.Player.Attack.performed += HandlePlayerInputAttack;

            startAttackEventBinding = new EventBinding<StartAttackEvent>(HandleStartAttackEvent);
            startAttackEventBinding.Add(HandleAttackStart);
            EventBus<StartAttackEvent>.Register(startAttackEventBinding);

            unitAttackFinishEventBinding = new EventBinding<UnitAttackFinishEvent>(HandleUnitAttackFinish);
            EventBus<UnitAttackFinishEvent>.Register(unitAttackFinishEventBinding);
        }

        private void OnDisable()
        {
            GameStateManager.OnAfterStateChanged -= OnAfterGameStateChanged;
            InputManager.Instance.PlayerInputActions.Player.Attack.performed -= HandlePlayerInputAttack;

            EventBus<StartAttackEvent>.Deregister(startAttackEventBinding);
            EventBus<UnitAttackFinishEvent>.Deregister(unitAttackFinishEventBinding);
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

        private void HandleAttackStart(StartAttackEvent startAttackEvent)
        {
            currentInstigator = startAttackEvent.CombatTeam;
            currentAttackFinishCount = 0;

            CheckFinishAttack();
        }

        private void HandleUnitAttackFinish(UnitAttackFinishEvent unitAttackFinishEvent)
        {
            currentAttackFinishCount += 1;

            // Debug.Log($"Count =  {currentAttackFinishCount} Length = {currentCombatTeamInfo.UnitList.Length}");
            CheckFinishAttack();
        }

        private void CheckFinishAttack()
        {
            UnitManager.CombatTeamInfo? combatTeamInfo =
                UnitManager.Instance.GetCombatTeam(currentInstigator);

            if (combatTeamInfo == null || currentAttackFinishCount == combatTeamInfo?.UnitList.Length)
            {
                FinishAttack();
            }
        }

        private async void FinishAttack()
        {
            if (currentInstigator == UnitManager.CombatTeam.Player)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(1));
                GameStateManager.Instance.ChangeState(GameState.EnemyTurn);
            }
            else if (currentInstigator == UnitManager.CombatTeam.Enemy)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(1));
                GameStateManager.Instance.ChangeState(GameState.HeroTurn);
            }
        }
    }
}