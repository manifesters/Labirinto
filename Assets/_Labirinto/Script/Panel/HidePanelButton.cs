using MainMenu;
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
            AudioManager.Instance.PlaySFX(AudioManager.Instance.button_click);
			Debug.Log("The button is clicked");
            _panelManager.HideLastPanel();
        }

        public void DestroyPanel()
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.button_click);
			Debug.Log("The Panel Destroyed");
            _panelManager.DestroyLastPanel();
        }
    }
}

