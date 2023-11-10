using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Utils;

namespace Systems
{
    public class SceneSystem : StaticInstance<SceneSystem>
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
    }
}