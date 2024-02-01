using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace SceneUtilities
{
    public class SceneLoader
    {
        private AsyncOperation asyncOperation;

        public static event System.Action ActivateLoading;
        public static event System.Func<float, bool> UpdateLoading;

        private static string nextSceneIndex;
        private string loadingScene;

        private readonly TransitionPlayer transitionPlayer;

        public SceneLoader(ref TransitionPlayer transitionPlayer)
        {
            this.transitionPlayer = transitionPlayer;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            EndLoad();

            if (SceneManager.GetActiveScene().name == loadingScene)
            {
                ActivateLoading?.Invoke();
                LoadSceneAsync();
            }
        }

        private async void EndLoad()
        {
            await transitionPlayer.endTransition?.Invoke();
        }

        public async void LoadScene(string sceneName, bool transition)
        {
            if(transition)
                await transitionPlayer.startTransition.Invoke();

            SceneManager.LoadScene(sceneName);
        }

        public async void LoadWithLoadingScreen(string nextScene, string loadingScene)
        {
            await transitionPlayer.startTransition.Invoke();
            nextSceneIndex = nextScene;
            this.loadingScene = loadingScene;
            SceneManager.LoadScene(loadingScene);
        }

        private async void LoadSceneAsync()
        {
            await Task.Delay(1000);
            asyncOperation = SceneManager.LoadSceneAsync(nextSceneIndex);
            asyncOperation.allowSceneActivation = false;

            while (UpdateLoading?.Invoke(asyncOperation.progress) == false)
                await Task.Delay(100);

            AllowScene();
        }

        private async void AllowScene()
        {
            await transitionPlayer.startTransition.Invoke();
            asyncOperation.allowSceneActivation = true;
        }
    }
}
