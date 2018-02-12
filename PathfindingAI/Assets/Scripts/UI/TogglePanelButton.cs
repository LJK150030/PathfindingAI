using UnityEngine;

namespace Assets.Scripts.UI
{
    public class TogglePanelButton : MonoBehaviour
    {
        public GameObject UIPanel;

        public void TogglePanel(GameObject panel)
        {
            panel.SetActive(!panel.activeSelf);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                TogglePanel(UIPanel);
        }
    }
}
