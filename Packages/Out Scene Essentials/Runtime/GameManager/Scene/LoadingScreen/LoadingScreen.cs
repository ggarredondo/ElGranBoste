using LerpUtilities;
using UnityEngine;
using UnityEngine.UI;

namespace SceneUtilities
{
    public class LoadingScreen : MonoBehaviour
    {
        [Header("Loading bar")]
        [SerializeField] private Image progressBar;
        [SerializeField] private float progressBarSpeed;
        [SerializeField] private Animator loadingTextAnim;

        private string continueAction;
        private float actualProgress;
        private string percentage;

        private void Start()
        {
            GameManager.Input.ControlsChangedEvent += ChangeText;
            ChangeText();
        }

        private void OnEnable()
        {
            SceneLoader.ActivateLoading += Activate;
            SceneLoader.UpdateLoading += UpdateProgess;
        }

        private void OnDisable()
        {
            SceneLoader.ActivateLoading -= Activate;
            SceneLoader.UpdateLoading -= UpdateProgess;
        }

        private void OnDestroy()
        {
            GameManager.Input.ControlsChangedEvent -= ChangeText;
        }

        private async void ProgressBar()
        {
            await Lerp.Value(progressBar.transform.localScale.x, actualProgress,
                                  (p) => progressBar.transform.localScale = new Vector3(p, progressBar.transform.localScale.y, progressBar.transform.localScale.z),
                                  progressBarSpeed);
        }

        public void Activate()
        {
            GameManager.Input.EnablePlayerInput(true);
            GameManager.Input.SwitchActionMap("LoadingScreen");
        }

        public bool UpdateProgess(float progress)
        {
            actualProgress = Mathf.Clamp01(progress / 0.9f);
            percentage = (int)(Mathf.Clamp01(progress / 0.9f) * 100) + " %";
            ProgressBar();

            bool result = false;

            if (progress >= 0.9f)
            {
                percentage = "Press " + continueAction + " to continue";
                loadingTextAnim.enabled = true;

                if (GameManager.Input.FindAction("Continue").IsPressed())
                {
                    result = true;
                }
            }

            return result;
        }

        public void ChangeText()
        {
            continueAction = GameManager.Input.FindAction("Continue").bindings[GameManager.Input.ControlSchemeIndex].path.Split("/")[1];
        }

    }
}
