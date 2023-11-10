using System;
using Systems;
using UnityEngine;

namespace UI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private CanvasGroup background;
        [SerializeField] private Transform container;
        [SerializeField] private float containerTargetYPosition = -72;

        private void Start()
        {
            SceneSystem.Instance.OnPauseStateChange += HandlePauseStateChange;
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            SceneSystem.Instance.OnPauseStateChange -= HandlePauseStateChange;
        }

        void HandlePauseStateChange(bool newState)
        {
            // Debug.Log($"HandlePauseStateChange new state = {newState}");

            if (newState)
            {
                gameObject.SetActive(true);

                background.alpha = 0;
                container.localPosition = new Vector2(0, containerTargetYPosition - Screen.height);

                background.LeanAlpha(1, 0.5f).setIgnoreTimeScale(true);
                container.LeanMoveLocalY(containerTargetYPosition, 0.5f).setEaseOutCubic().setIgnoreTimeScale(true);
            }
            else
            {
                background.LeanAlpha(0, 0.5f).setIgnoreTimeScale(true);
                container.LeanMoveLocalY(containerTargetYPosition - Screen.height, 0.5f)
                    .setEaseInCubic()
                    .setIgnoreTimeScale(true)
                    .setOnComplete(() => gameObject.SetActive(false));
            }
        }
    }
}