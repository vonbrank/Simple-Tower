using System;
using Systems;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LoadingCanvas : MonoBehaviour
    {
        [SerializeField] private Image loadingProgressBarInner;

        private bool isFirstUpdate = true;


        private void Update()
        {
            if (isFirstUpdate)
            {
                SceneSystem.Instance.LoadingSceneCallback();
                isFirstUpdate = false;
            }

            loadingProgressBarInner.fillAmount = SceneSystem.Instance.GetLoadingProgress();
        }
    }
}