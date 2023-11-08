using System;
using System.Collections;
using Attributes;
using UnityEngine;

namespace Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float lifeSpan = 30;
        [SerializeField] private float speed = 2;

        private Health target;

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

        public void SetTarget(Health target)
        {
            this.target = target;
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
                // Debug.Log($"Projectile Hit on {other.gameObject.name}");
                // Debug.Log("fuck");
                Destroy(gameObject);
            }
        }
    }
}