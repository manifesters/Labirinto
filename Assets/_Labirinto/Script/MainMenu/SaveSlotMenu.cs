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
		AudioManager audioManager;

		private void Awake() 
        {
            saveSlots = this.GetComponentsInChildren<SaveSlot>();
			audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
		}

        public void OnSaveSlotClicked(SaveSlot saveSlot) 
        {
			audioManager.PlaySFX(audioManager.uiButton);
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
                    "Starting a New Game with this slot will override the currently saved data. Are you sure?",
                    // function to execute if we select 'yes'
                    () => {
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
                        SaveGameAndLoadScene();
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
            DisableMenuButtons();

            confirmationPopupMenu.ActivateMenu(
                "Are you sure you want to delete this saved data?",
                // function to execute if we select 'yes'
                () => {
                    DataPersistenceManager.Instance.DeleteProfileData(saveSlot.GetProfileId());
                    ActivateMenu(isLoadingGame);
                },
                // function to execute if we select 'cancel'
                () => {
                    ActivateMenu(isLoadingGame);
                }
            );
        }

        public void OnBackClicked() 
        {
			audioManager.PlaySFX(audioManager.uiButton);
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
