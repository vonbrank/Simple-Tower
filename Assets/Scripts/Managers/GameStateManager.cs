using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Utils;
using Utils.Event;


namespace Managers
{
    public class GameStateManager : StaticInstance<GameStateManager>
    {
        public static event Action<GameState> OnBeforeStateChanged;
        public static event Action<GameState> OnAfterStateChanged;

        private void Start()
        {
            ChangeState(GameState.Start);
        }

        public GameState GameState { get; private set; }

        public void ChangeState(GameState newState)
        {
            OnBeforeStateChanged?.Invoke(GameState);

            GameState = newState;

            switch (GameState)
            {
                case GameState.Start:
                    HandleStart();
                    break;
                case GameState.HeroTurn:
                    HandleHeroTurn();
                    break;
                case GameState.EnemyTurn:
                    HandleEnemyTurn();
                    break;
                case GameState.Win:
                    HandlePlayerWin();
                    break;
                case GameState.Lose:
                    HandlePlayerLose();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }

            OnAfterStateChanged?.Invoke(GameState);

            // Debug.Log($"New state: {newState}");
        }

        private async void HandleStart()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            for (int i = 3; i >= -1; i--)
            {
                EventBus<StartCountDownEvent>.Raise(new StartCountDownEvent
                {
                    CountDownNumber = i
                });

                if (i >= 0)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(1));
                }
            }

            ChangeState(GameState.HeroTurn);
        }

        private void HandleHeroTurn()
        {
            // TODO: Let player operate
        }

        private void HandleEnemyTurn()
        {
            EventBus<StartAttackEvent>.Raise(new StartAttackEvent
            {
                CombatTeam = UnitManager.CombatTeam.Enemy
            });
        }

        private void HandlePlayerWin()
        {
        }

        private void HandlePlayerLose()
        {
        }
    }

    public enum GameState
    {
        Start,
        HeroTurn,
        EnemyTurn,
        Win,
        Lose,
    }
}