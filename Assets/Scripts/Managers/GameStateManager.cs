using System;
using System.Threading.Tasks;
using UnityEngine;
using Utils;


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
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }

            OnAfterStateChanged?.Invoke(GameState);

            Debug.Log($"New state: {newState}");
        }

        private async void HandleStart()
        {
            await Task.Yield();
            ChangeState(GameState.HeroTurn);
        }

        private void HandleHeroTurn()
        {
            // TODO: Let player operate
        }

        private void HandleEnemyTurn()
        {
            UnitManager.Instance.StartAttack(UnitManager.AttackInstigator.Enemy);
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