using System;
using Attributes;
using Managers;
using UnityEngine;

namespace Combat
{
    public class Fighter : MonoBehaviour
    {
        // [SerializeField] private GameObject target;
        [SerializeField] private Projectile projectilePrefab;
        [SerializeField] private GameObject ProjectileSpawnPoint;

        private void Awake()
        {
        }

        public void Attack(Health target)
        {
            var projectile = Instantiate(projectilePrefab, ProjectileSpawnPoint.transform.position,
                Quaternion.identity);
            projectile.SetTarget(target);
        }
    }
}