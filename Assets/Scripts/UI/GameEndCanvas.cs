using System;
using Managers;
using Systems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameEndCanvas : MonoBehaviour
    {
        [SerializeField] private Button RestartButton;
        [SerializeField] private Button QuitButton;
        [SerializeField] private TextMeshProUGUI GameEndText;

        private void Start()
        {
            RestartButton.onClick.AddListener(HandleRestart);
            QuitButton.onClick.AddListener(HandleQuit);
            GameStateManager.OnAfterStateChanged += OnGameStateChange;

            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            RestartButton.onClick.RemoveListener(HandleRestart);
            QuitButton.onClick.RemoveListener(HandleQuit);
            GameStateManager.OnAfterStateChanged -= OnGameStateChange;
        }


        private void OnGameStateChange(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.Win:
                    gameObject.SetActive(true);
                    GameEndText.text = "You win the game!";
                    break;
                case GameState.Lose:
                    gameObject.SetActive(true);
                    GameEndText.text = "You lose the game!";
                    break;
                default:
                    break;
            }
        }

        private void HandleRestart()
        {
            SceneSystem.Instance.LoadScene(SceneSystem.Scene.MainLevel);
        }

        private void HandleQuit()
        {
            SceneSystem.Instance.LoadScene(SceneSystem.Scene.MainMenu);
        }
    }
}