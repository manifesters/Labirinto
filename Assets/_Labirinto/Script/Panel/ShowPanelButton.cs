using UnityEngine;

namespace Panel
{
    public class ShowPanelButton : MonoBehaviour
    {
		AudioManager audioManager;
		public void Awake()
		{
			audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
		}
		public string PanelID;

        public PanelShowBehaviour Behaviour;

        private PanelManager _panelManager;

        void Start()
        {
            _panelManager = PanelManager.Instance;
        }

        public void DoShowPanel()
        {
			audioManager.PlaySFX(audioManager.uiButton);
			Debug.Log("The button is clicked");
            _panelManager.ShowPanel(PanelID, Behaviour);
        }
    }   
}
