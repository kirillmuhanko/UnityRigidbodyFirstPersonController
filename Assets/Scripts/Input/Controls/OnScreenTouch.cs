using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

namespace Input.Controls
{
    [AddComponentMenu("Input/On-Screen Touch")]
    public class OnScreenTouch : OnScreenControl, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [InputControl(layout = "Vector2")] [SerializeField]
        private string actionMap;

        private Vector2 _lastPosition;

        public void OnBeginDrag(PointerEventData eventData)
        {
            _lastPosition = GetLocalPointInRectangle(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            var position = GetLocalPointInRectangle(eventData);
            SendValueToControl(position - _lastPosition);
            _lastPosition = position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            SendValueToControl(Vector2.zero);
        }

        protected override string controlPathInternal
        {
            get => actionMap;
            set => actionMap = value;
        }

        private Vector2 GetLocalPointInRectangle(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                transform.parent.GetComponentInParent<RectTransform>(),
                eventData.position,
                eventData.pressEventCamera,
                out var localPoint);

            return localPoint;
        }
    }
}