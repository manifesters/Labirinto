using System.Collections.Generic;
using DataPersistence;
using Helper;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainMenu
{
    public class SaveSlotsMenu : MonoBehaviour
    {
        [Header("Menu Navigation")]
        [SerializeField] private MainMenu mainMenu;

        [Header("Menu Buttons")]
        [SerializeField] private Button backButton;

        [Header("Confirmation Popup")]
        [SerializeField] private ConfirmationPopupMenu confirmationPopupMenu;
        [SerializeField] private SavedNamePopupMenu savedNamePopupMenu;

        private SaveSlot[] saveSlots;

        private bool isLoadingGame = false;

		private void Awake() 
        {
            saveSlots = this.GetComponentsInChildren<SaveSlot>();
		}

        public void OnSaveSlotClicked(SaveSlot saveSlot) 
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.button_click);
			DisableMenuButtons();

            // if the game is loading
            if (isLoadingGame) 
            {
                DataPersistenceManager.Instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
                SaveGameAndLoadScene();
            }
            // new game but has already a game data
            else if (saveSlot.hasData) 
            {
                confirmationPopupMenu.ActivateMenu(
                    "Mabubura ang kasalukuyang naka-save na data. Sigurado ka?",
                    // function to execute if we select 'yes'
                    () => {
                        AudioManager.Instance.PlaySFX(AudioManager.Instance.button_click);
                        savedNamePopupMenu.ActivateMenu(
                            "Enter save name",
                            (inputText) => {
                                // Handle the inputText when "Confirm" is pressed
                                Debug.Log("User entered: " + inputText);
                                // Proceed with game logic using inputText
                                DataPersistenceManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
                                DataPersistenceManager.instance.NewGame(inputText);
                                SaveGameAndLoadScene();
                            },
                            // function to execute if we select 'cancel'
                            () => {
                                Debug.Log("Cancelled");
                                this.ActivateMenu(isLoadingGame); // Re-activate if needed
                            }
                        );
                    },
                    // function to execute if we select 'cancel'
                    () => {
                        AudioManager.Instance.PlaySFX(AudioManager.Instance.button_click);
                        this.ActivateMenu(isLoadingGame);
                    }
                );
            }
            // new game in slot whithout data
            else 
            {
                savedNamePopupMenu.ActivateMenu(
                    "Enter save name",
                    (inputText) => {
                        // Handle the inputText when "Confirm" is pressed
                        Debug.Log("User entered: " + inputText);
                        // Proceed with game logic using inputText
                        DataPersistenceManager.Instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
                        DataPersistenceManager.Instance.NewGame(inputText);
                        DataPersistenceManager.Instance.SaveGame();
                        GameManager.Instance.LoadScene("StartStory", GameState.STORYMODE);
                        //SaveGameAndLoadScene();
                    },
                    // function to execute if we select 'cancel'
                    () => {
                        Debug.Log("Cancelled");
                        this.ActivateMenu(isLoadingGame); // Re-activate if needed
                    }
                );
            }
        }

        private void SaveGameAndLoadScene() 
        {
            // save the game anytime before loading a new scene
            DataPersistenceManager.Instance.SaveGame();
            GameManager.Instance.LoadScene("Labirinto", GameState.LABIRINTO);
            SceneManager.LoadSceneAsync("Labirinto");
        }

        public void OnClearClicked(SaveSlot saveSlot) 
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.button_click);
            DisableMenuButtons();

            confirmationPopupMenu.ActivateMenu(
                "Sigurado ka bang nais mong burahin?",
                // function to execute if we select 'yes'
                () => {
                    AudioManager.Instance.PlaySFX(AudioManager.Instance.button_click);
                    DataPersistenceManager.Instance.DeleteProfileData(saveSlot.GetProfileId());
                    ActivateMenu(isLoadingGame);
                },
                // function to execute if we select 'cancel'
                () => {
                    AudioManager.Instance.PlaySFX(AudioManager.Instance.button_click);
                    ActivateMenu(isLoadingGame);
                }
            );
        }

        public void OnBackClicked() 
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.button_click);
			mainMenu.ActivateMenu();
            this.DeactivateMenu();
        }

        public void ActivateMenu(bool isLoadingGame) 
        {
            // set this menu to be active
            this.gameObject.SetActive(true);

            // set mode
            this.isLoadingGame = isLoadingGame;

            // load all of the profiles that exist
            Dictionary<string, GameData> profilesGameData = DataPersistenceManager.Instance.GetAllProfilesGameData();

            // ensure the back button is enabled when we activate the menu
            backButton.interactable = true;

            foreach (SaveSlot saveSlot in saveSlots) 
            {
                GameData profileData = null;
                profilesGameData.TryGetValue(saveSlot.GetProfileId(), out profileData);
                saveSlot.SetData(profileData);
                if (profileData == null && isLoadingGame) 
                {
                    saveSlot.SetInteractable(false);
                }
                else 
                {
                    saveSlot.SetInteractable(true);
                }
            }
        }

        public void DeactivateMenu() 
        {
            this.gameObject.SetActive(false);
        }

        private void DisableMenuButtons() 
        {
            foreach (SaveSlot saveSlot in saveSlots) 
            {
                saveSlot.SetInteractable(false);
            }
            backButton.interactable = false;
        }
    }
}
