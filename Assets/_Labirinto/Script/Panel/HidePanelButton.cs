using MainMenu;
using UnityEngine;

namespace Panel
{
    public class HidePanelButton : MonoBehaviour
    {
        private PanelManager _panelManager;

		AudioManager audioManager;

		private void Awake()
		{
			audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
		}

		void Start()
        {
            _panelManager = PanelManager.Instance;
        }

        public void DoHidePanel()
        {
			audioManager.PlaySFX(audioManager.uiButton);
			Debug.Log("The button is clicked");
            _panelManager.HideLastPanel();
        }

        public void DestroyPanel()
        {
			audioManager.PlaySFX(audioManager.uiButton);
			Debug.Log("The Panel Destroyed");
            _panelManager.DestroyLastPanel();
        }
    }
}

