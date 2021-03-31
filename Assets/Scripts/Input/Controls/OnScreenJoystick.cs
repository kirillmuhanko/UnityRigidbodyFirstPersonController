using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

namespace Input.Controls
{
    [AddComponentMenu("Input/On-Screen Joystick")]
    public class OnScreenJoystick : OnScreenControl, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private float movementRange = 50f;
        public RectTransform stickBackgroundTransform;
        public RectTransform stickTransform;

        [InputControl(layout = "Vector2")] [SerializeField]
        private string actionMap;

        private Vector2 _beginDragPosition;
        private Vector3 _startPosition;

        public void OnBeginDrag(PointerEventData eventData)
        {
            stickBackgroundTransform.position = eventData.position;
            stickTransform.position = eventData.position;
            _beginDragPosition = GetLocalPointInRectangle(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            var position = GetLocalPointInRectangle(eventData);
            var delta = position - _beginDragPosition;

            delta = Vector2.ClampMagnitude(delta, movementRange);
            stickTransform.anchoredPosition = delta;

            var value = new Vector2(delta.x / movementRange, delta.y / movementRange);
            SendValueToControl(value);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            stickBackgroundTransform.anchoredPosition = _startPosition;
            stickTransform.anchoredPosition = Vector2.zero;
            SendValueToControl(Vector2.zero);
        }

        protected override string controlPathInternal
        {
            get => actionMap;
            set => actionMap = value;
        }

        private void Start()
        {
            _startPosition = stickBackgroundTransform.anchoredPosition;
        }

        private Vector2 GetLocalPointInRectangle(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                stickBackgroundTransform,
                eventData.position,
                eventData.pressEventCamera,
                out var localPoint);

            return localPoint;
        }
    }
}