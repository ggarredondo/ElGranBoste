using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace InputUtilities
{
    public class InputFacade
    {
        private static InputMapping inputMapping;
        private static InputDetection inputDetection;
        private static ControllerRumble controllerRumble;
        private static PlayerInput playerInput;
        private static InputSystemUIInputModule uiInput;

        private bool updateInput;
        private string pauseControlScheme;
        private UnityEngine.InputSystem.InputDevice pauseInputdevice;
        private int currentPlayer;

        private System.Action detectionMethod;

        public InputFacade(InputSystemUIInputModule uiModule)
        {
            uiInput = uiModule;

            inputMapping = new();
            inputDetection = new();
            controllerRumble = new();

            EnterPauseMenu(0,1);
        }

        public async void OnPlayerJoined(PlayerInput currentPlayerInput)
        {
            currentPlayerInput.DeactivateInput();

            await System.Threading.Tasks.Task.Delay(System.TimeSpan.FromSeconds(1));

            currentPlayerInput.ActivateInput();
        }

        public void EnterPauseMenu(int index, int maxPlayerInput)
        {
            for(int i = 0; i < maxPlayerInput; i++)
            {
                if (i == index)
                {
                    playerInput = PlayerInput.all[index];
                    currentPlayer = index;
                }
                else
                    PlayerInput.all[i].DeactivateInput();
            }

            SwitchActionMap("UI");

            pauseControlScheme = CurrentControlScheme();
            pauseInputdevice = CurrentInputDevice();

            uiInput.actionsAsset = playerInput.actions;

            updateInput = true;

            if (pauseControlScheme == "Gamepad")
                detectionMethod = inputDetection.GamepadDetection;
            else
                detectionMethod = inputDetection.KeyboardMouseDetection;
        }

        public void ExitPauseMenu(int maxPlayerInput)
        {
            updateInput = false;

            for (int i = 0; i < maxPlayerInput; i++)
                PlayerInput.all[i].ActivateInput();

            if (pauseControlScheme == "Gamepad")
                playerInput.SwitchCurrentControlScheme(pauseControlScheme, pauseInputdevice);
            else
                playerInput.SwitchCurrentControlScheme(pauseControlScheme, Keyboard.current, Mouse.current);

            SwitchActionMap("Main Movement");
        }

        public int CheckControlScheme(int index)
        {
            PlayerInput playerInput = PlayerInput.all[index];

            return inputDetection.CheckControlScheme(playerInput);
        }

        public void UpdateInputMapping()
        {
            inputDetection.UpdateInputMapping();
        }

        public void Update()
        {
            detectionMethod();
        }

        public void Rumble(int deviceID, float duration, float leftAmplitude, float rightAmplitude)
        {
            controllerRumble.Rumble(deviceID, duration, leftAmplitude, rightAmplitude);
        }

        public void AddGamepadRumble(int deviceID)
        {
            controllerRumble.AddGamepad(deviceID);
        }

        public void SwitchActionMap(string newActionMap)
        {
            playerInput.SwitchCurrentActionMap(newActionMap);
        }

        public void SetSelectedGameObject(GameObject obj)
        {
            if(obj != null)
                inputDetection.selected = obj;
        }

        #region ACCESS

        public ref readonly bool UpdateInput => ref updateInput;

        public ref readonly PlayerInput PlayerInput => ref playerInput;

        public ref readonly int ControlSchemeIndex => ref inputDetection.controlSchemeIndex;

        public ref readonly InputDevice PreviousCustomControlScheme => ref inputDetection.previousCustomControlScheme; 

        public ref System.Action ControlsChangedEvent => ref inputDetection.controlsChangedEvent;

        public bool NeedToSelect => inputDetection.controlSchemeIndex == 0 || inputDetection.previousCustomControlScheme == InputDevice.KEYBOARD;

        public void EnablePlayerInput(bool enable)
        {
            playerInput.enabled = enable;
        }

        public void ActivatePlayerInput(bool activate)
        {
            for (int i = 0; i < PlayerInput.all.Count; i++)
            {
                if (activate) PlayerInput.all[i].ActivateInput();
                else PlayerInput.all[i].DeactivateInput();
            }
        }

        public void EnableUIModule(bool enable)
        {
            if (uiInput != null)
                uiInput.enabled = enable;
        }

        public string CurrentControlScheme()
        {
            return playerInput.currentControlScheme;
        }

        public UnityEngine.InputSystem.InputDevice CurrentInputDevice()
        {
            return playerInput.devices[0];
        }

        public InputAction FindAction(string action)
        {
            return playerInput.actions.FindAction(action);
        }

        #endregion

        #region MAPPING

        public void LoadUserRebinds(ref PlayerInput playerInput, int index)
        {
            playerInput.actions.LoadBindingOverridesFromJson(GameManager.Save.Options.rebinds[index]);
        }

        public void SaveUserRebinds()
        {
            GameManager.Save.Options.rebinds[currentPlayer] = playerInput.actions.SaveBindingOverridesAsJson();
        }

        public string ObtainAllowedMapping(in string buttonName)
        {
            string path = playerInput.actions.FindAction(buttonName).bindings[inputDetection.controlSchemeIndex].effectivePath;

            return inputMapping.ObtainAllowedMapping(path);
        }

        public string ObtainAllowedMapping(in InputAction action)
        {
            string path = action.bindings[inputDetection.controlSchemeIndex].effectivePath;

            return inputMapping.ObtainAllowedMapping(path);
        }


        public string ObtainMapping(in string buttonName, int index = 0)
        {
            PlayerInput playerInput = PlayerInput.all[index];

            string path = playerInput.actions.FindAction(buttonName).bindings[CheckControlScheme(index)].effectivePath;

            return inputMapping.ObtainMapping(path);
        }

        #endregion
    }
}
