using System;
using TMPro;
using Unit;
using UnityEngine;

namespace UI.InGame
{
    public class HealthDisplay : MonoBehaviour
    {
        private UnitBase unitBase;

        [SerializeField] private TextMeshProUGUI healthText;

        private void Awake()
        {
        }

        private void OnEnable()
        {
            unitBase = GetComponentInParent<UnitBase>();
            unitBase.Health.OnHealthUpdate += OnHealthUpdate;
        }

        private void OnDisable()
        {
            unitBase.Health.OnHealthUpdate -= OnHealthUpdate;
        }

        private void OnHealthUpdate(int health)
        {
            healthText.text = $"{health}/{unitBase.Health.MaxHealth}";
        }
    }
}