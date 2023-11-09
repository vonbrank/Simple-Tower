using System;
using System.Collections;
using Attributes;
using Managers;
using Unit;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float lifeSpan = 30;
        [SerializeField] private float speed = 2;
        [SerializeField] private int damageAmount = 10;

        private UnitBase target;
        private UnitManager.CombatTeam instigatorTeam;

        public event Action OnProjectileDestroy;

        private void Start()
        {
            StartCoroutine(DestroyOnLifeSpan());
            transform.LookAt(target.transform.position);
        }

        IEnumerator DestroyOnLifeSpan()
        {
            yield return new WaitForSeconds(lifeSpan);
            Destroy(gameObject);
        }

        public void SetTarget(UnitBase target, UnitManager.CombatTeam instigatorTeam)
        {
            this.target = target;
            this.instigatorTeam = instigatorTeam;
        }

        private void Update()
        {
            transform.LookAt(target.transform.position);
            transform.position += transform.forward * (speed * Time.deltaTime);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                var health = other.gameObject.GetComponent<Health>();
                if (health)
                {
                    health.TakeDamage(
                        Mathf.Max(0,
                            Random.Range(Mathf.RoundToInt(damageAmount * 0.5f), Mathf.RoundToInt(damageAmount * 1.5f))),
                        instigatorTeam);
                }

                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            OnProjectileDestroy?.Invoke();
        }
    }
}