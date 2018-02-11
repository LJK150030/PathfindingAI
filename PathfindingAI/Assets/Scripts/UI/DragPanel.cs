using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI
{
    public class DragPanel : MonoBehaviour, IPointerDownHandler, IDragHandler
    {

        private Vector2 _pointerOffset;
        private RectTransform _canvasRectTransform;
        private RectTransform _panelRectTransform;

        void Awake()
        {
            Canvas canvas = GetComponentInParent<Canvas>();

            //grabing refrences of the canvas and the panel
            if (canvas != null)
            {
                _canvasRectTransform = canvas.transform as RectTransform;
                _panelRectTransform = transform.parent as RectTransform;
            }
        }

        public void OnPointerDown(PointerEventData data)
        {
            //will be in front of any other UI object
            _panelRectTransform.SetAsLastSibling();
            
            //where did my pointer came down on the panel, finding it locacly on the panel
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_panelRectTransform, data.position,
                data.pressEventCamera, out _pointerOffset);
        }

        public void OnDrag(PointerEventData data)
        {
            if (_panelRectTransform == null)
                return;

            Vector2 pointerPosition = ClampToWindow(data);

            Vector2 localPointerPosition;

            if(RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRectTransform, pointerPosition, data.pressEventCamera, out localPointerPosition))
            {
                _panelRectTransform.localPosition = localPointerPosition - _pointerOffset;
            }
        }

        private Vector2 ClampToWindow(PointerEventData data)
        {
            Vector2 rawPointerPosition = data.position;

            Vector3[] canvasCorners = new Vector3[4];
            _canvasRectTransform.GetWorldCorners(canvasCorners);

            float clampedX = Mathf.Clamp(rawPointerPosition.x, canvasCorners[0].x, canvasCorners[2].x);
            float clampedY = Mathf.Clamp(rawPointerPosition.y, canvasCorners[0].y, canvasCorners[2].y);

            Vector2 newPointerPosition = new Vector2(clampedX, clampedY);
            return newPointerPosition;
        }
    }
}
