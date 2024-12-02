using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace MainMenu
{
    public class SavedNamePopupMenu : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private TextMeshProUGUI displayText;
        [SerializeField] private TMP_InputField nameInputField;
        [SerializeField] private Button confirmButton;
        [SerializeField] private Button cancelButton;

        public void ActivateMenu(string displayText, UnityAction<string> confirmAction, UnityAction cancelAction)
        {
            this.gameObject.SetActive(true);

            // Ensure the input field is cleared or has a default value
            nameInputField.text = "";

            // Remove any existing listeners to avoid duplication
            confirmButton.onClick.RemoveAllListeners();
            cancelButton.onClick.RemoveAllListeners();

            // Assign listeners
            confirmButton.onClick.AddListener(() => {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.button_click);
                string inputText = nameInputField.text;

                if (!string.IsNullOrEmpty(inputText)) 
                {
                    confirmAction(inputText);
                    DeactivateMenu();
                }
                else
                {
                    Debug.Log("Player's name cannot be empty!");
                }
            });

            cancelButton.onClick.AddListener(() => {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.button_click);
                cancelAction();
                DeactivateMenu();
            });
        }

        private void DeactivateMenu()
        {
            this.gameObject.SetActive(false);
        }
    }
}

