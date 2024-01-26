using System.Collections.Generic;
using UnityEngine;

namespace SceneUtilities
{
    [CreateAssetMenu(fileName = "SceneLogic", menuName = "Scriptable Objects/Scene/Scene Logic")]
    public class SceneLogic : ScriptableObject
    {
        [System.Serializable]
        public struct SceneData
        {
            public string sceneName;
            public bool withLoadScreen;
            public string loadSceneName;
        }

        [Header("Scene")]
        public string sceneName;
        public List<SceneData> nextScene;
        public List<SceneData> previousScene;

        [Header("Sound")]
        public bool playMusic;
        [ConditionalField("playMusic")] public string music;

        [Header("Objects Initialization")]
        public List<string> gameObjectsTag;
    }
}
