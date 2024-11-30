using DataPersistence;
using Helper;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainMenu
{
    public class MainMenu : MonoBehaviour
    {

        [Header("Menu Navigation")]
        [SerializeField] private SaveSlotsMenu saveSlotsMenu;

        [Header("Menu Buttons")]
        [SerializeField] private Button newGameButton;
        [SerializeField] private Button continueGameButton;
        [SerializeField] private Button loadGameButton;

		AudioManager audioManager;
		public void Awake()
		{
			audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
		}

		private void Start() 
        {
            DisableButtonsDependingOnData();
        }

        private void DisableButtonsDependingOnData() 
        {
            if (!DataPersistenceManager.Instance.HasGameData()) 
            {
                continueGameButton.interactable = false;
                loadGameButton.interactable = false;
            }
        }

        public void OnNewGameClicked() 
        {
			audioManager.PlaySFX(audioManager.uiButton);
			saveSlotsMenu.ActivateMenu(false);
            this.DeactivateMenu();
        }

        public void OnLoadGameClicked() 
        {
			audioManager.PlaySFX(audioManager.uiButton);
			saveSlotsMenu.ActivateMenu(true);
            this.DeactivateMenu();
        }

        public void OnContinueGameClicked() 
        {
			audioManager.PlaySFX(audioManager.uiButton);
			DisableMenuButtons();
            // save the game anytime before loading a new scene
            DataPersistenceManager.Instance.SaveGame();
            // load the next scene - which will in turn load the game because of 
            // OnSceneLoaded() in the DataPersistenceManager

            Debug.LogWarning("No last saved scene found. Loading default scene.");
            GameManager.Instance.LoadScene("Labirinto", GameState.LABIRINTO);
        }

        private void DisableMenuButtons() 
        {
            newGameButton.interactable = false;
            continueGameButton.interactable = false;
        }

        public void ActivateMenu() 
        {
			this.gameObject.SetActive(true);
			audioManager.PlaySFX(audioManager.uiButton);
			DisableButtonsDependingOnData();
        }

        public void DeactivateMenu() 
        {
			this.gameObject.SetActive(false);
			audioManager.PlaySFX(audioManager.uiButton);
		}
    }    
}
