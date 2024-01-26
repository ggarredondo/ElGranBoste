using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace InputUtilities
{
    public class InputDetection
    {
        public int controlSchemeIndex;
        public GameObject selected;

        public System.Action controlsChangedEvent;

        public InputDevice previousCustomControlScheme = InputDevice.UNKNOW;

        
        public void KeyboardMouseDetection()
        {
            InputDevice currentControlScheme = DetectInputDevice();

            if (currentControlScheme != previousCustomControlScheme && currentControlScheme != InputDevice.UNKNOW)
            {
                previousCustomControlScheme = currentControlScheme;
                SetCustomControlScheme();
                OnControlSchemeChanged(GameManager.Input.PlayerInput);
            }
        }

        private InputDevice DetectInputDevice()
        {
            if (Keyboard.current != null && Keyboard.current.anyKey.isPressed)
                return InputDevice.KEYBOARD;
            else if (Mouse.current != null && Mouse.current.delta.ReadValue() != Vector2.zero)
                return InputDevice.MOUSE;

            return InputDevice.UNKNOW;
        }

        private void SetCustomControlScheme()
        {
            if (PlayerInput.all.Count > 0)
            {
                if (previousCustomControlScheme == InputDevice.KEYBOARD)
                {
                    EventSystem.current.SetSelectedGameObject(selected);
                    Cursor.visible = false;
                }
                else if (previousCustomControlScheme == InputDevice.MOUSE)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                    Cursor.visible = true;
                }
            }
        }

        public void GamepadDetection()
        {
            InputDevice currentControlScheme = InputDevice.GAMEPAD;

            if (currentControlScheme != previousCustomControlScheme && currentControlScheme != InputDevice.UNKNOW)
            {
                previousCustomControlScheme = currentControlScheme;
                EventSystem.current.SetSelectedGameObject(selected);
                Cursor.visible = false;
                OnControlSchemeChanged(GameManager.Input.PlayerInput);
            }
        }

        public void OnControlSchemeChanged(PlayerInput playerInput)
        {
            switch (playerInput.currentControlScheme)
            {
                case "Keyboard&Mouse":
                    controlSchemeIndex = 1;
                    break;
                case "Gamepad":
                    controlSchemeIndex = 0;
                    break;
            }

            controlsChangedEvent?.Invoke();
        }

        public int CheckControlScheme(PlayerInput playerInput)
        {
            int controlSchemeIndex = 0;

            switch (playerInput.currentControlScheme)
            {
                case "Keyboard&Mouse":
                    controlSchemeIndex = 1;
                    break;
                case "Gamepad":
                    controlSchemeIndex = 0;
                    break;
            }

            return controlSchemeIndex;
        }

        public void UpdateInputMapping()
        {
            OnControlSchemeChanged(GameManager.Input.PlayerInput);
        }
    }
}

public enum InputDevice
{
    GAMEPAD,
    KEYBOARD,
    MOUSE,
    UNKNOW
}
