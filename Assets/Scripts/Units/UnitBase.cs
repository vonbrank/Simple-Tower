using System;
using Attributes;
using Combat;
using Managers;
using UnityEngine;

namespace Unit
{
    public interface IUnit
    {
        public UnitManager.CombatTeam CombatTeam { get; }
    }

    public class UnitBase : MonoBehaviour, IUnit
    {
        public Health Health { get; private set; }
        public Fighter Fighter { get; private set; }


        [SerializeField] private UnitManager.CombatTeam combatTeam;

        public UnitManager.CombatTeam CombatTeam
        {
            get => combatTeam;
        }

        private void Awake()
        {
            Health = GetComponent<Health>();
            Fighter = GetComponent<Fighter>();
        }

        private void OnEnable()
        {
            Health.OnDie += HandleUnitDied;
        }

        private void OnDisable()
        {
            Health.OnDie -= HandleUnitDied;
        }

        private void HandleUnitDied(UnitManager.CombatTeam instigator)
        {
            UnitManager.Instance.UnitDied(this);
        }
    }
}