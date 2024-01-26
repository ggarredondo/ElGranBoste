namespace UnityEngine.Rendering.Universal
{
    [CreateAssetMenu(fileName = "MyURPAsset", menuName = "Scriptable Objects/Visuals/MyURPAsset")]
    public class MyURPAsset : ScriptableObject
    {
        [Header("Requirements")]
        public int ID;
        public VisualData data;

        [Header("Options")]
        public int antiAliasing;
        public bool castShadows;
        public int shadowDistance;
        public int shadowResolution;
        public int shadowCascade;
        public int textureResolution;
        public bool softShadows;
        public bool anisotropic;
        public bool softParticles;

        public UniversalRenderPipelineAsset urpAsset;

        public void ApplyChanges()
        {
            urpAsset.msaaSampleCount = data.antialiasingMap[antiAliasing];
            urpAsset.shadowDistance = data.shadowDistanceMap[shadowDistance];
            urpAsset.shadowCascadeCount = data.shadowCascadeMap[shadowCascade];

            QualitySettings.SetQualityLevel(ID, true);

            UnityGraphics.MainLightCastShadows = castShadows;
            UnityGraphics.AdditionalLightCastShadows = castShadows;

            UnityGraphics.MainLightShadowResolution = data.shadowResolutionMap[shadowResolution].shadowResolution;
            UnityGraphics.AdditionalLightShadowResolution = data.shadowResolutionMap[shadowResolution].shadowResolution;
            UnityGraphics.AdditionalLightsShadowResolutionTierHigh = data.shadowResolutionMap[shadowResolution].tierHigh;
            UnityGraphics.AdditionalLightsShadowResolutionTierMedium = data.shadowResolutionMap[shadowResolution].tierMid;
            UnityGraphics.AdditionalLightsShadowResolutionTierLow = data.shadowResolutionMap[shadowResolution].tierLow;

            UnityGraphics.SoftShadowsEnabled = softShadows;

            QualitySettings.globalTextureMipmapLimit = data.textureResolutionMap[textureResolution];
            QualitySettings.anisotropicFiltering = anisotropic ? AnisotropicFiltering.Enable : AnisotropicFiltering.Disable;
            QualitySettings.softParticles = softParticles;
        }
    }
}
