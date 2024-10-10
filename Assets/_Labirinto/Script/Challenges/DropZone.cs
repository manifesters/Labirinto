using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class DropZone : MonoBehaviour, IDropHandler
{
    [SerializeField] private string correctWord;
    private TextMeshProUGUI dropZoneText;
    private RectTransform dropZoneRect;
    private bool isOccupied = false;

    void Awake()
    {
        dropZoneText = GetComponentInChildren<TextMeshProUGUI>();
        dropZoneText.text = "Drop Here"; // Placeholder
        dropZoneRect = GetComponent<RectTransform>();
    }

    // logic for dropping the words and what would happen
    public void OnDrop(PointerEventData eventData)
    {
        if (isOccupied) return; // Don't accept new words if already occupied

        DraggableWord draggableWord = eventData.pointerDrag.GetComponent<DraggableWord>();

        if (draggableWord != null)
        {
            TextMeshProUGUI wordText = draggableWord.GetComponent<TextMeshProUGUI>();

            // Checking if the drop occurred within the drop zone holder
            if (RectTransformUtility.RectangleContainsScreenPoint(dropZoneRect, eventData.position, eventData.pressEventCamera))
            {
                if (wordText.text == correctWord)
                {
                    dropZoneText.text = wordText.text;
                    dropZoneText.color = Color.green;
                    isOccupied = true; // Mark the drop zone as occupied
                    Destroy(draggableWord.gameObject); // Remove the draggable word

                    // Notify the GameController of the correct answer
                    FindObjectOfType<GameController>().CorrectAnswerPlaced();
                }
                else
                {
                    dropZoneText.color = Color.red;
                    draggableWord.SetColor(Color.red); // Change the color to red if incorrect
                    draggableWord.ResetPosition();

                    // Notify the GameController of the incorrect answer to deduct time
                    FindObjectOfType<GameController>().IncorrectAnswerPlaced();
                }
            }
            else
            {
                // Return the word to its original position if dropped outside the drop zone holder
                draggableWord.ResetPosition();
            }
        }
    }
}

