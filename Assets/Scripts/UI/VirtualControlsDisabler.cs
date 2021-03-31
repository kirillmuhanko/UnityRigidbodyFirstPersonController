using UnityEngine;

namespace UI
{
    public class VirtualControlsDisabler : MonoBehaviour
    {
        protected void OnEnable()
        {
#if (!UNITY_ANDROID && !UNITY_IOS) || UNITY_EDITOR
            gameObject.SetActive(false);
#endif
        }
    }
}