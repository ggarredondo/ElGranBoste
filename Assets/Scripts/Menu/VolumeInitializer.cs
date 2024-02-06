using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VolumeInitializer : MonoBehaviour
{
    [SerializeField] private List<VolumeProfile> volumeProfiles;
    private List<ColorAdjustments> colorAdjustments;

    private void Start()
    {
        colorAdjustments = new();

        for (int i = 0; i < volumeProfiles.Count; i++)
        {
            if (volumeProfiles[i].TryGet(out ColorAdjustments tmp)) colorAdjustments.Add(tmp);
        }

        foreach (ColorAdjustments color in colorAdjustments)
        {
            color.postExposure.value = GameManager.Save.Options.brightness;
            color.saturation.value = GameManager.Save.Options.saturation;
        }
    }
}
