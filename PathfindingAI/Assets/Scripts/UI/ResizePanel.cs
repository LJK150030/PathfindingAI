using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI
{
    public class ResizePanel : MonoBehaviour, IPointerDownHandler, IDragHandler
    {

        public Vector2 MinSize;
        public Vector2 MaxSize;

        private RectTransform _rectTransform;
        private Vector2 _currentPointerPosition;
        private Vector2 _previousPointerPosition;

        //grabbing the parent and moive the parent, NOT the hotspot
        void Awake()
        {
            _rectTransform = transform.parent.GetComponent<RectTransform>();
        }

        //making the parent object the focus, set the previous point position
        public void OnPointerDown(PointerEventData data)
        {
            _rectTransform.SetAsLastSibling();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, data.position, data.pressEventCamera, out _previousPointerPosition);
        }

        //draging the corner
        public void OnDrag(PointerEventData data)
        {
            if (_rectTransform == null)
                return;

            Vector2 sizeDelta = _rectTransform.sizeDelta;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, data.position, data.pressEventCamera, out _currentPointerPosition);
            Vector2 resizeValue = _currentPointerPosition - _previousPointerPosition;

            sizeDelta += new Vector2(resizeValue.x, -resizeValue.y);
            sizeDelta = new Vector2(
                Mathf.Clamp(sizeDelta.x, MinSize.x, MaxSize.x),
                Mathf.Clamp(sizeDelta.y, MinSize.y, MaxSize.y)
            );

            _rectTransform.sizeDelta = sizeDelta;

            _previousPointerPosition = _currentPointerPosition;
        }
    }
}