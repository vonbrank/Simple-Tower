using System;
using Combat;
using Managers;
using UnityEngine;
using Utils.Event;

namespace Control
{
    [RequireComponent(typeof(Fighter))]
    public class ControllerBase : MonoBehaviour
    {
        protected Fighter fighter;

        protected EventBinding<StartAttackEvent> startAttackEventBinding;

        private void OnEnable()
        {
            startAttackEventBinding = new EventBinding<StartAttackEvent>(HandleStartAttack);
            EventBus<StartAttackEvent>.Register(startAttackEventBinding);
        }

        private void OnDisable()
        {
            EventBus<StartAttackEvent>.Deregister(startAttackEventBinding);
        }

        protected void Awake()
        {
            fighter = GetComponent<Fighter>();
        }

        protected virtual void HandleStartAttack(StartAttackEvent startAttackEvent)
        {
        }
    }
}