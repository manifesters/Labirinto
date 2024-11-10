using UnityEngine;

namespace Panel
{
    public class HidePanelButton : MonoBehaviour
    {
        private PanelManager _panelManager;

        void Start()
        {
            _panelManager = PanelManager.Instance;
        }

        public void DoHidePanel()
        {
            Debug.Log("The button is clicked");
            _panelManager.HideLastPanel();
        }

        public void DestroyPanel()
        {
            Debug.Log("The Panel Destroyed");
            _panelManager.DestroyLastPanel();
        }
    }
}

