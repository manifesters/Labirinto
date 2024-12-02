using UnityEngine;

namespace Panel
{
    public class ShowPanelButton : MonoBehaviour
    {
		public string PanelID;

        public PanelShowBehaviour Behaviour;

        private PanelManager _panelManager;

        void Start()
        {
            _panelManager = PanelManager.Instance;
        }

        public void DoShowPanel()
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.button_click);
			Debug.Log("The button is clicked");
            _panelManager.ShowPanel(PanelID, Behaviour);
        }
    }   
}
