using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic; // Needed for List

public class GuessPicController : MonoBehaviour

{
    public Image pictureDisplay; // Reference to the picture display
    public Button[] optionButtons; // References to the option buttons
    public PictureData[] pictureDataArray; // Array of PictureData assets

    public Color correctColor = Color.green; // Color for correct answer
    public Color wrongColor = Color.red; // Color for wrong answer
    public Color defaultColor = Color.white; // Default button color

    private List<PictureData> remainingQuestions; // List to track remaining questions
    private PictureData currentPictureData;

    void Start()
    {
        // Check for necessary assignments
        if (pictureDataArray == null || pictureDataArray.Length == 0)
        {
            Debug.LogError("PictureDataArray is empty or missing. Assign PictureData assets in the Inspector.");
            return;
        }

        if (optionButtons == null || optionButtons.Length == 0)
        {
            Debug.LogError("OptionButtons array is empty or not assigned. Assign buttons in the Inspector.");
            return;
        }

        if (pictureDisplay == null)
        {
            Debug.LogError("PictureDisplay is not assigned. Drag the UI Image to the Inspector.");
            return;
        }

        // Initialize the list of remaining questions
        remainingQuestions = new List<PictureData>(pictureDataArray);

        LoadNewQuestion();
    }

    void LoadNewQuestion()
    {
        // Check if there are remaining questions
        if (remainingQuestions.Count == 0)
        {
            Debug.Log("All questions answered!");
            // You can trigger some final logic here, such as displaying a "game over" screen
            return;
        }

        // Reset all buttons to default color
        foreach (Button btn in optionButtons)
        {
            btn.GetComponent<Image>().color = defaultColor;
        }

        // Select a random question from the remaining questions
        int randomIndex = Random.Range(0, remainingQuestions.Count);
        currentPictureData = remainingQuestions[randomIndex];

        // Remove the selected question from the list to avoid repetition
        remainingQuestions.RemoveAt(randomIndex);

        // Validate the picture and options
        if (currentPictureData.picture == null)
        {
            Debug.LogError("currentPictureData.picture is null. Assign a sprite to the PictureData asset.");
            return;
        }

        if (currentPictureData.options == null || currentPictureData.options.Length < optionButtons.Length)
        {
            Debug.LogError("Options in currentPictureData are null or not enough for the buttons.");
            return;
        }

        // Set the picture on the display
        pictureDisplay.sprite = currentPictureData.picture;

        // Loop through each button to set the text and add listeners
        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (i >= currentPictureData.options.Length)
            {
                Debug.LogError($"Index {i} exceeds options array length.");
                return;
            }

            // Access the TextMeshProUGUI component instead of the Text component
            var textComponent = optionButtons[i].GetComponentInChildren<TMP_Text>(); // Change to TMP_Text
            if (textComponent == null)
            {
                Debug.LogError($"Button {i + 1} is missing a TMP_Text component.");
                return;
            }

            // Set the button text
            textComponent.text = currentPictureData.options[i];

            // Add a listener to the button to check if the selected option is correct
            int index = i; // Capture index for use in the listener
            optionButtons[i].onClick.RemoveAllListeners(); // Clear previous listeners
            optionButtons[i].onClick.AddListener(() => OnOptionSelected(index));
        }
    }

    // Handle the selection of an option
    void OnOptionSelected(int selectedIndex)
    {
        // Check if the answer is correct or wrong and change button colors
        if (selectedIndex == currentPictureData.correctOptionIndex)
        {
            Debug.Log("Correct Answer!");
            optionButtons[selectedIndex].GetComponent<Image>().color = correctColor; // Set to green if correct
        }
        else
        {
            Debug.Log("Wrong Answer!");
            optionButtons[selectedIndex].GetComponent<Image>().color = wrongColor; // Set to red if incorrect
        }

        // Disable all buttons after answer selection
        foreach (Button btn in optionButtons)
        {
            btn.interactable = false;
        }

        // Wait for 1 second to show the color feedback, then load the next question and reset colors
        Invoke("ResetAndLoadNewQuestion", 1f);
    }

    // Reset button colors and load a new question
    void ResetAndLoadNewQuestion()
    {
        // Reset all buttons to default color
        foreach (Button btn in optionButtons)
        {
            btn.GetComponent<Image>().color = defaultColor;
            btn.interactable = true; // Re-enable buttons for the next question
        }

        // Load the next question
        LoadNewQuestion();
    }
}
