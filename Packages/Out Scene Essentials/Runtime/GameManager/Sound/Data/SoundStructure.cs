using System;
using UnityEngine;
using UnityEngine.Audio;

namespace AudioUtilities
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Sound/SoundStructure")]
    public class SoundStructure : ScriptableObject
    {
        [Header("AudioMixer")]
        public AudioMixerGroup audioMixerGroup;

        [Header("3D Sound")]
        public bool threeD;

        [ConditionalField("threeD")] [Range(0, 5)] public float dopplerLevel = 1f;
        [ConditionalField("threeD")] [Range(0, 360)] public float spread = 0f;
        [ConditionalField("threeD")] public AudioRolloffMode volumeRolloff = AudioRolloffMode.Logarithmic;
        [ConditionalField("threeD")] public float minDistance = 1f;
        [ConditionalField("threeD")] public float maxDistance = 500f;
        [ConditionalField("threeD")] public AnimationCurve customRollofCurve;

        [Header("Manager Sounds")]
        [SerializeField] private SoundGroup[] soundGroups;

        public SoundGroup[] SoundGroups { get { return soundGroups; } }

        public string GetStructureName()
        {
            return audioMixerGroup.name;
        }
    }
}
