using System;
using Systems;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainMenuCanvas : MonoBehaviour
    {
        [SerializeField] private Button startButton;

        private void OnEnable()
        {
            startButton.onClick.AddListener(HandleStartButtonClick);
        }

        private void OnDisable()
        {
            startButton.onClick.RemoveListener(HandleStartButtonClick);
        }

        private void HandleStartButtonClick()
        {
            SceneSystem.Instance.LoadScene(SceneSystem.Scene.MainLevel);
        }
    }
}