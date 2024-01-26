
namespace SceneUtilities
{
    public interface IChangeScene
    {
        public void NextScene(int sceneIndex = 0, bool transition = true);
        public void PreviousScene(int sceneIndex = 0, bool transition = true);
        public string GetCurrentLoadScene();
        public SceneLogic GetSceneLogic(string name);
    }
}
