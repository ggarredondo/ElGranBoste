using System.Collections.Generic;

namespace SaveUtilities
{
    [System.Serializable]
    public class OptionsSlot : SaveSlot
    {
        public bool mute;
        public float masterVolume;
        public float musicVolume;
        public float sfxVolume;

        public bool vSync;
        public bool fullscreen;
        public string resolution;
        public int quality;

        public float brightness;
        public float saturation;

        public bool rumble;
        public List<string> rebinds;

        public new object Clone()
        {
            OptionsSlot nuevo = (OptionsSlot)MemberwiseClone();

            nuevo.rebinds = new List<string>(rebinds);

            return nuevo;
        }
    }
}
