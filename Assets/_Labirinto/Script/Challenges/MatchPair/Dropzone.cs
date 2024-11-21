using MatchChallenge;
using UnityEngine;

public class Dropzone : MonoBehaviour
{
    private MatchGameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<MatchGameManager>();
    }

    // Called when an item is dropped into this zone
    public void OnItemDropped(GameObject draggedItem)
    {
        if (draggedItem != null)
        {
            // Get the corresponding pair (find the matching item)
            GameObject targetItem = transform.GetChild(0).gameObject; // The corresponding item on this side of the match

            // Notify the game manager to check if the match is correct
            gameManager.CheckMatch(draggedItem, targetItem);
        }
    }
}
