using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputActionRebindingExtensions;

namespace InputUtilities
{
    public class InputRemapping
    {
        private InputAction action, originalAction;

        public System.Action startRemap;
        public System.Action endRemap;

        public void Remapping(float rebindTimeDelay, string name)
        {
            action = GameManager.Input.FindAction(name);

            if (action == null)
                Debug.Log("This action not exists");
            else
            {
                startRemap.Invoke();

                int controlSchemeIndex = GameManager.Input.ControlSchemeIndex;

                originalAction = action.Clone();

                action.PerformInteractiveRebinding(controlSchemeIndex)
                    .WithControlsExcluding("Mouse")
                    .OnMatchWaitForAnother(rebindTimeDelay)
                    .OnCancel(callback => CancelRebind(callback, controlSchemeIndex))
                    .OnComplete(callback => FinishRebind(callback, controlSchemeIndex))
                    .Start();
            }
        }

        private void CancelRebind(RebindingOperation callback, int controlSchemeIndex)
        {
            callback.action.ApplyBindingOverride(controlSchemeIndex, originalAction.bindings[controlSchemeIndex]);
        }

        private void FinishRebind(RebindingOperation callback, int controlSchemeIndex)
        {
            endRemap.Invoke();

            if (GameManager.Input.ObtainAllowedMapping(callback.action) != "")
            {
                InputAction result = CheckIfAsigned(callback.action, controlSchemeIndex);
                if (result != null && !result.bindings[controlSchemeIndex].isComposite)
                {
                    result.ApplyBindingOverride(controlSchemeIndex, "");
                }
            }
            else callback.Cancel();

            GameManager.Input.SaveUserRebinds();
            callback.Dispose();
            GameManager.Input.UpdateInputMapping();
        }

        private InputAction CheckIfAsigned(InputAction action, int controlSchemeIndex)
        {
            InputAction result = null;
            InputBinding actualBinding = action.bindings[controlSchemeIndex];

            foreach (InputBinding binding in action.actionMap.bindings)
            {

                if (binding.action == actualBinding.action)
                {
                    continue;
                }

                if (binding.effectivePath == actualBinding.effectivePath)
                {
                    result = GameManager.Input.FindAction(binding.action);
                    break;
                }
            }

            return result;
        }
    }
}
