using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Text))]
    public class FpsCounter : MonoBehaviour
    {
        private float _deltaTime;
        private Text _counter;

        private void Awake()
        {
            _counter = GetComponent<Text>();
        }

        private void Update()
        {
            _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
            var ms = _deltaTime * 1000f;
            var fps = 1f / _deltaTime;
            _counter.text = $"{ms:0.0} ms ({fps:0.} fps)";
        }
    }
}