using System;
using Input;
using Systems;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace Managers
{
    public class InputManager : StaticInstance<InputManager>
    {
        public PlayerInputActions PlayerInputActions { get; private set; }

        protected override void Awake()
        {
            base.Awake();


            PlayerInputActions = new PlayerInputActions();
            PlayerInputActions.Player.Enable();
        }

        private void OnEnable()
        {
            SceneSystem.Instance.OnPauseStateChange += OnPauseStateChange;

            PlayerInputActions.Player.Pause.performed += HandlePlayerEscape;
            PlayerInputActions.UI.Resume.performed += HandleUiResume;
        }

        private void OnDisable()
        {
            SceneSystem.Instance.OnPauseStateChange -= OnPauseStateChange;

            PlayerInputActions.Player.Pause.performed -= HandlePlayerEscape;
            PlayerInputActions.UI.Resume.performed -= HandleUiResume;
        }

        // public void Attack(InputAction.CallbackContext context)
        // {
        //     Debug.Log($"{context}");
        // }

        private void OnPauseStateChange(bool isPaused)
        {
            if (isPaused)
            {
                PlayerInputActions.Player.Disable();
                PlayerInputActions.UI.Enable();
            }
            else
            {
                PlayerInputActions.Player.Enable();
                PlayerInputActions.UI.Disable();
            }
        }

        private void HandlePlayerEscape(InputAction.CallbackContext context)
        {
            SceneSystem.Instance.Pause();
        }

        private void HandleUiResume(InputAction.CallbackContext context)
        {
            SceneSystem.Instance.Resume();
        }
    }
}