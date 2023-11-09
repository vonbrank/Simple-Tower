using System;
using Attributes;
using Combat;
using Managers;
using UnityEngine;

namespace Unit
{
    public class UnitBase : MonoBehaviour
    {
        public Health Health { get; private set; }
        public Fighter Fighter { get; private set; }

        public UnitManager.CombatTeam CombatTeam { get; set; }

        private void Awake()
        {
            Health = GetComponent<Health>();
            Fighter = GetComponent<Fighter>();
        }
    }
}