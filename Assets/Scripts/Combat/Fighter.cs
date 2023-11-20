using System;
using Attributes;
using Managers;
using Unit;
using UnityEngine;
using Utils.Event;

namespace Combat
{
    public class Fighter : MonoBehaviour
    {
        // [SerializeField] private GameObject target;
        [SerializeField] private Projectile projectilePrefab;
        [SerializeField] private GameObject ProjectileSpawnPoint;

        private IUnit unit;

        private void Awake()
        {
            unit = GetComponent<IUnit>();
        }

        public void Attack(UnitBase target)
        {
            var projectile = Instantiate(projectilePrefab, ProjectileSpawnPoint.transform.position,
                Quaternion.identity);
            projectile.SetTarget(target, unit.CombatTeam);
            projectile.OnProjectileDestroy += () => EventBus<UnitAttackFinishEvent>.Raise(new UnitAttackFinishEvent
            {
                CombatTeam = unit.CombatTeam
            });
        }
    }
}