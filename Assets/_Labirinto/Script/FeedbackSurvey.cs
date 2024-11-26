using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI; // For Toggle and ToggleGroup
using TMPro; // For TMP_InputField
public class FeedbackSurvey : MonoBehaviour
{
    // Reference to the TMP_InputField for feedback
    [SerializeField] private TMP_InputField feedbackField;

    // Reference to the ToggleGroup
    [SerializeField] private ToggleGroup toggleGroup;

    private string URL = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSfENeMZrloGuoVBExYgFgYs1_0hjh4cSwIWyj7z6oaAT6tzmg/formResponse";

    // Method to send feedback
    public void Send()
    {
        // Check if a toggle is selected
        Toggle selectedToggle = GetSelectedToggle();
        if (selectedToggle == null)
        {
            Debug.LogWarning("Please select a rating before submitting.");
            return;
        }

        // Get the rating from the selected toggle
        int rating = int.Parse(selectedToggle.name);
        if (!string.IsNullOrEmpty(feedbackField.text))
        {
            StartCoroutine(Post(feedbackField.text, rating));
        }
        else
        {
            Debug.LogWarning("Feedback is empty. Please enter some feedback before submitting.");
        }
    }

    // Coroutine to post feedback data
    private IEnumerator Post(string feedback, int rating)
    {
        // Create a form and add fields
        WWWForm form = new WWWForm();
        form.AddField("entry.1679856434", feedback);
        form.AddField("entry.1236881915", rating.ToString());

        // Send the POST request
        UnityWebRequest www = UnityWebRequest.Post(URL, form);

        // Wait for the request to complete
        yield return www.SendWebRequest();

        // Check for errors
        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Feedback submitted successfully!");
            ResetForm(); // Reset the form upon successful submission
        }
        else
        {
            Debug.LogError($"Error submitting feedback: {www.error}");
        }
    }

    // Helper method to get the selected toggle
    private Toggle GetSelectedToggle()
    {
        foreach (var toggle in toggleGroup.ActiveToggles())
        {
            if (toggle.isOn)
            {
                return toggle;
            }
        }
        return null;
    }

    // Method to reset the form
    private void ResetForm()
    {
        // Clear the feedback field
        feedbackField.text = "";

        // Reset the toggle group (deselect all toggles)
        foreach (var toggle in toggleGroup.GetComponentsInChildren<Toggle>())
        {
            toggle.isOn = false;
        }
    }
}
