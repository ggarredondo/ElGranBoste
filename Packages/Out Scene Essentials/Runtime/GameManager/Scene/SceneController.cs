using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneUtilities
{
    public class SceneController : IChangeScene
    {
        private Dictionary<string, SceneLogic> scenesTable;
        private List<SceneLogic> scenes;
        private readonly SceneLoader sceneLoader;

        private string currentScene;
        private string currentLoadScene;

        public SceneController(string initialScene, List<SceneLogic> scenes, ref TransitionPlayer transitionPlayer)
        {
            Application.backgroundLoadingPriority = ThreadPriority.Low;

            sceneLoader = new(ref transitionPlayer);
            scenesTable = new();

            currentScene = initialScene;

            this.scenes = scenes;
            Initialize();
        }

        private void Initialize()
        {
            scenes.ForEach(s => scenesTable.Add(s.sceneName, s));
        }

        private void UpdateScene(string nextScene)
        {
            currentScene = nextScene;
        }

        public List<string> GetInitializeTags(Scene scene)
        {
            return scenesTable[scene.name].gameObjectsTag;
        }

        public void NextScene(int sceneIndex = 0, bool transition = true)
        {
            string nextScene = scenesTable[currentScene].nextScene[sceneIndex].sceneName;

            if (scenesTable[currentScene].nextScene[sceneIndex].withLoadScreen)
            {
                currentLoadScene = scenesTable[currentScene].nextScene[sceneIndex].loadSceneName;
                sceneLoader.LoadWithLoadingScreen(nextScene, currentLoadScene);
            }
            else
                sceneLoader.LoadScene(nextScene, transition);

            UpdateScene(nextScene);
        }

        public void PreviousScene(int sceneIndex = 0, bool transition = true)
        {
            string nextScene = scenesTable[currentScene].previousScene[sceneIndex].sceneName;

            if (scenesTable[currentScene].previousScene[sceneIndex].withLoadScreen)
            {
                currentLoadScene = scenesTable[currentScene].previousScene[sceneIndex].loadSceneName;
                sceneLoader.LoadWithLoadingScreen(nextScene, currentLoadScene);
            }
            else
                sceneLoader.LoadScene(nextScene, transition);

            UpdateScene(nextScene);
        }

        public string GetCurrentLoadScene()
        {
            return currentLoadScene;
        }

        public SceneLogic GetSceneLogic(string name)
        {
            return scenesTable[currentScene];
        }
    }
}
