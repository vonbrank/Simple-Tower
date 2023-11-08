using System;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainCanvas : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI gameStateText;
        [SerializeField] private Button attackButton;

        private void Start()
        {
            attackButton.interactable = false;
        }

        private void OnEnable()
        {
            GameStateManager.OnAfterStateChanged += OnAfterGameStateChanged;
            GameStateManager.OnBeforeStateChanged += OnBeforeGameStateChanged;
            attackButton.onClick.AddListener(HandleAttackButtonClicked);
        }

        private void OnDisable()
        {
            GameStateManager.OnAfterStateChanged -= OnAfterGameStateChanged;
            GameStateManager.OnBeforeStateChanged -= OnBeforeGameStateChanged;
            attackButton.onClick.RemoveListener(HandleAttackButtonClicked);
        }

        private void OnAfterGameStateChanged(GameState newGameState)
        {
            if (newGameState == GameState.HeroTurn)
            {
                attackButton.interactable = true;
            }
        }

        private void OnBeforeGameStateChanged(GameState currentGameState)
        {
            if (currentGameState == GameState.HeroTurn)
            {
                attackButton.interactable = false;
            }
        }

        private void HandleAttackButtonClicked()
        {
            UnitManager.Instance.StartAttack(UnitManager.AttackInstigator.Player);
            attackButton.interactable = false;
        }
    }
}