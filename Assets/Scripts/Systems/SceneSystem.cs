using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Systems
{
    public class SceneSystem : Singleton<SceneSystem>
    {
        public bool IsPaused { get; private set; }
        public event Action<bool> OnPauseStateChange;

        protected override void Awake()
        {
            base.Awake();
            IsPaused = false;
            OnPauseStateChange?.Invoke(IsPaused);
        }

        private void Start()
        {
            // TestPause();
        }

        private async void TestPause()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(7), true);

            Pause();

            await UniTask.Delay(TimeSpan.FromSeconds(3), true);

            Resume();
        }

        public void Pause()
        {
            if (IsPaused)
            {
                return;
            }

            Time.timeScale = 0;
            IsPaused = true;
            OnPauseStateChange?.Invoke(IsPaused);
        }

        public void Resume()
        {
            if (!IsPaused)
            {
                return;
            }

            Time.timeScale = 1;
            IsPaused = false;
            OnPauseStateChange?.Invoke(IsPaused);
        }

        public enum Scene
        {
            MainMenu,
            LoadingScene,
            MainLevel
        }

        private event Action OnLoadingSceneCallback;
        private AsyncOperation loadingAsyncOperation;

        public void LoadScene(Scene scene)
        {
            OnLoadingSceneCallback += () => StartCoroutine(LoadSceneAsync(scene));

            SceneManager.LoadScene(Scene.LoadingScene.ToString());
        }

        public void LoadingSceneCallback()
        {
            OnLoadingSceneCallback?.Invoke();
            OnLoadingSceneCallback = null;
        }

        private IEnumerator LoadSceneAsync(Scene scene)
        {
            yield return new WaitForSeconds(0.5f);

            loadingAsyncOperation = SceneManager.LoadSceneAsync(scene.ToString());
            loadingAsyncOperation.allowSceneActivation = false;

            do
            {
                yield return null;
            } while (loadingAsyncOperation.progress < 0.9f);

            yield return new WaitForSeconds(0.5f);

            loadingAsyncOperation.allowSceneActivation = true;
            loadingAsyncOperation = null;
        }

        public float GetLoadingProgress()
        {
            if (loadingAsyncOperation != null)
            {
                return loadingAsyncOperation.progress;
            }

            return 0;
        }
    }
}