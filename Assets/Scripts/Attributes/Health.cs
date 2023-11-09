using System;
using Managers;
using UnityEngine;

namespace Attributes
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 100;

        public int MaxHealth
        {
            get { return maxHealth; }
        }

        public int CurrentHealth { get; private set; }

        public event Action<int> OnHealthUpdate;

        private void Start()
        {
            CurrentHealth = maxHealth;
            OnHealthUpdate?.Invoke(CurrentHealth);
        }


        public void TakeDamage(int damageAmount, UnitManager.CombatTeam instigatorTeam)
        {
            CurrentHealth -= damageAmount;
            OnHealthUpdate?.Invoke(CurrentHealth);
        }
    }
}