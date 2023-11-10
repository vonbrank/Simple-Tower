using System;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.Event;

namespace UI
{
    public class MainCanvas : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI gameStateText;
        [SerializeField] private Button attackButton;

        [Header("Start Counting Down")] [SerializeField]
        private GameObject startCountingDownContainer;

        [SerializeField] private TextMeshProUGUI startCountingDownText;

        private EventBinding<StartCountDownEvent> startCountDownEventBinding;


        private void Start()
        {
            attackButton.interactable = false;
            startCountingDownContainer.SetActive(true);
        }

        private void OnEnable()
        {
            GameStateManager.OnAfterStateChanged += OnAfterGameStateChanged;
            GameStateManager.OnBeforeStateChanged += OnBeforeGameStateChanged;
            CombatManager.OnCombatInfoUpdate += HandleCombatInfoUpdate;

            attackButton.onClick.AddListener(HandleAttackButtonClicked);

            startCountDownEventBinding = new EventBinding<StartCountDownEvent>(HandleStartCountingDownEvent);
            EventBus<StartCountDownEvent>.Register(startCountDownEventBinding);
        }

        private void OnDisable()
        {
            GameStateManager.OnAfterStateChanged -= OnAfterGameStateChanged;
            GameStateManager.OnBeforeStateChanged -= OnBeforeGameStateChanged;
            CombatManager.OnCombatInfoUpdate -= HandleCombatInfoUpdate;

            attackButton.onClick.RemoveListener(HandleAttackButtonClicked);

            EventBus<StartCountDownEvent>.Deregister(startCountDownEventBinding);
        }

        private void OnAfterGameStateChanged(GameState newGameState)
        {
            gameStateText.text = $"Game State: {newGameState}";
        }

        private void OnBeforeGameStateChanged(GameState currentGameState)
        {
        }

        private void HandleAttackButtonClicked()
        {
            EventBus<StartAttackEvent>.Raise(new StartAttackEvent
            {
                CombatTeam = UnitManager.CombatTeam.Player
            });
        }

        private void HandleStartCountingDownEvent(StartCountDownEvent startCountDownEvent)
        {
            startCountingDownText.text = startCountDownEvent.CountDownNumber switch
            {
                < 0 => "",
                0 => "Go!",
                _ => $"{startCountDownEvent.CountDownNumber}",
            };

            if (startCountDownEvent.CountDownNumber < 0)
            {
                startCountingDownContainer.SetActive(false);
            }
        }

        void HandleCombatInfoUpdate(CombatManager.CombatInfo combatInfo)
        {
            attackButton.interactable = combatInfo.PlayerCanAttack;
        }
    }
}