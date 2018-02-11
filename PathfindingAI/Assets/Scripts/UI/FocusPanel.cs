using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI
{
    public class FocusPanel : MonoBehaviour, IPointerDownHandler
    {

        private RectTransform _panel;
        private CanvasGroup _canvasGroup;

        void Awake()
        {
            _panel = GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.blocksRaycasts = true;
        }

        public void OnPointerDown(PointerEventData data)
        {
            _panel.SetAsLastSibling();
        }

    }
}