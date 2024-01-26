using UnityEngine.InputSystem;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace InputUtilities
{
    public class ControllerRumble
    {
        private Dictionary<int, bool> rumbling;
        private Dictionary<int, Gamepad> gamepad;

        public ControllerRumble()
        {
            gamepad = new();
            rumbling = new();
        }

        public void AddGamepad(int deviceID)
        {
            if (InputSystem.devices[deviceID] is Gamepad gamepad)
            {
                if (this.gamepad.ContainsKey(deviceID))
                {
                    this.gamepad[deviceID] = gamepad;
                    rumbling[deviceID] = false;
                    return;
                }

                this.gamepad.Add(deviceID, gamepad);
                rumbling.Add(deviceID, false);
            }
        }

        public void Rumble(int deviceID, float duration, float leftAmplitude, float rightAmplitude)
        {
            if (gamepad.ContainsKey(deviceID) && !rumbling[deviceID] && GameManager.Save.Options.rumble)
            {
                gamepad[deviceID].SetMotorSpeeds(leftAmplitude, rightAmplitude);
                rumbling[deviceID] = true;
                StopRumble(deviceID ,duration);
            }
        }

        private async void StopRumble(int deviceID, float duration)
        {
            await Task.Delay(System.TimeSpan.FromSeconds(duration));
            gamepad[deviceID].SetMotorSpeeds(0f, 0f);
            rumbling[deviceID] = false;
        }
    }
}