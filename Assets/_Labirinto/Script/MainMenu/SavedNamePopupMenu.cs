using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

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
            string inputText = nameInputField.text; // Get the text from the input field

            if (!string.IsNullOrEmpty(inputText)) // Ensure the input text is not blank
            {
                confirmAction(inputText);
                DeactivateMenu();
            }
            else
            {
                Debug.Log("Player's name cannot be empty!"); // Optional: Add a prompt for empty input
            }
        });

        cancelButton.onClick.AddListener(() => {
            cancelAction();
            DeactivateMenu();
        });
    }

    private void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }
}
