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
            AudioManager.Instance.PlaySFX(AudioManager.Instance.button_click);
			saveSlotsMenu.ActivateMenu(false);
            this.DeactivateMenu();
        }

        public void OnLoadGameClicked() 
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.button_click);
			saveSlotsMenu.ActivateMenu(true);
            this.DeactivateMenu();
        }

        public void OnContinueGameClicked() 
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.button_click);
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
            AudioManager.Instance.PlaySFX(AudioManager.Instance.button_click);
			this.gameObject.SetActive(true);
			DisableButtonsDependingOnData();
        }

        public void DeactivateMenu() 
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.button_click);
			this.gameObject.SetActive(false);
		}
    }    
}
