using DataPersistence;
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
            saveSlotsMenu.ActivateMenu(false);
            this.DeactivateMenu();
        }

        public void OnLoadGameClicked() 
        {
            saveSlotsMenu.ActivateMenu(true);
            this.DeactivateMenu();
        }

        public void OnContinueGameClicked() 
        {
            DisableMenuButtons();
            // save the game anytime before loading a new scene
            DataPersistenceManager.Instance.SaveGame();
            // load the next scene - which will in turn load the game because of 
            // OnSceneLoaded() in the DataPersistenceManager
            string lastSceneName = DataPersistenceManager.Instance.GetLastSavedSceneName();
            if (!string.IsNullOrEmpty(lastSceneName))
            {
                SceneManager.LoadSceneAsync(lastSceneName);
            }
            else
            {
                Debug.LogWarning("No last saved scene found. Loading default scene.");
                SceneManager.LoadSceneAsync("Labirinto");
            }
        }

        private void DisableMenuButtons() 
        {
            newGameButton.interactable = false;
            continueGameButton.interactable = false;
        }

        public void ActivateMenu() 
        {
            this.gameObject.SetActive(true);
            DisableButtonsDependingOnData();
        }

        public void DeactivateMenu() 
        {
            this.gameObject.SetActive(false);
        }
    }    
}
