using System;
using Managers;
using UnityEngine;

namespace Control
{
    public class PlayerController : ControllerBase
    {
        private void Start()
        {
            // fighter.Attack(UnitManager.Instance.SelectEnemyUnit());
        }

        private void OnEnable()
        {
            UnitManager.Instance.OnPlayerStartAttack += OnStartAttack;
        }

        private void OnDisable()
        {
            UnitManager.Instance.OnPlayerStartAttack -= OnStartAttack;
        }

        private void OnStartAttack()
        {
            fighter.Attack(UnitManager.Instance.SelectEnemyUnit());
        }
    }
}