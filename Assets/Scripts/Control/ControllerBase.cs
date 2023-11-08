using System;
using Combat;
using UnityEngine;

namespace Control
{
    [RequireComponent(typeof(Fighter))]
    public class ControllerBase : MonoBehaviour
    {
        protected Fighter fighter;

        protected void Awake()
        {
            fighter = GetComponent<Fighter>();
        }
    }
}