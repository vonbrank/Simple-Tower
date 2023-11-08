using System;
using System.Threading.Tasks;
using Attributes;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Managers
{
    public class UnitManager : StaticInstance<UnitManager>
    {
        [SerializeField] private Health[] friendlyUnits;
        [SerializeField] private Health[] enemyUnits;

        public event Action OnPlayerStartAttack;

        public Health SelectFriendlyUnit()
        {
            return friendlyUnits[Random.Range(0, friendlyUnits.Length)];
        }

        public Health SelectEnemyUnit()
        {
            return enemyUnits[Random.Range(0, enemyUnits.Length)];
        }

        public enum AttackInstigator
        {
            Player,
            Enemy,
        }

        public async void StartAttack(AttackInstigator attackInstigator)
        {
            if (attackInstigator == AttackInstigator.Player)
            {
                OnPlayerStartAttack?.Invoke();
                await Task.Delay(TimeSpan.FromSeconds(3));
                GameStateManager.Instance.ChangeState(GameState.EnemyTurn);
            }
            else
            {
                await Task.Delay(TimeSpan.FromSeconds(3));
                GameStateManager.Instance.ChangeState(GameState.HeroTurn);
            }
        }
    }
}