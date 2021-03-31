using UnityEngine;

namespace Managers
{
    public class GraphicsQualityManager : MonoBehaviour
    {
        private void Awake()
        {
            QualitySettings.SetQualityLevel(0, true);
            QualitySettings.vSyncCount = 1;
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
            // Application.targetFrameRate = 60;
        }
    }
}