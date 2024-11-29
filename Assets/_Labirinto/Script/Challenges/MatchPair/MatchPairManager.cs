using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
using Challenge;

namespace MatchPairGame
{
    public class MatchGameManager : MonoBehaviour
    {
        [Header("Game Elements")]
        [SerializeField] private GameObject draggableItemsPanel;
        [SerializeField] private GameObject slotPanel;
        [SerializeField] private GameObject draggableItemPrefab;
        [SerializeField] private GameObject slotPrefab;
        [SerializeField] private Button submitButton;
        [SerializeField] private TextMeshProUGUI challengeName;

        private Dictionary<string, string> pairs = new Dictionary<string, string>();

        void Start()
        {
            LoadGameFromChallengeManager();
            submitButton.onClick.AddListener(CheckMatches);
        }

        public void LoadGameFromChallengeManager()
        {
            // Get the game JSON from the ChallengeManager
            TextAsset gameJson = ChallengeManager.Instance.dataJson;

            if (gameJson != null)
            {
                LoadGame(gameJson);
            }
            else
            {
                Debug.LogError("No game JSON found in ChallengeManager!");
            }
        }

        public void LoadGame(TextAsset gameJson)
        {
            if (gameJson != null)
            {
                string jsonText = gameJson.text;
                MatchData matchData = JsonConvert.DeserializeObject<MatchData>(jsonText);

                challengeName.text = matchData.challengeName;

                // Populate draggable items and slots
                foreach (var pair in matchData.pairs)
                {
                    pairs[pair.item] = pair.pair;

                    // Create draggable item
                    GameObject draggableItem = Instantiate(draggableItemPrefab, draggableItemsPanel.transform);
                    draggableItem.GetComponentInChildren<TextMeshProUGUI>().text = pair.item;

                    // Create slot
                    GameObject slot = Instantiate(slotPrefab, slotPanel.transform);
                    slot.GetComponentInChildren<TextMeshProUGUI>().text = pair.pair;
                    slot.name = pair.pair; // Use pair as the slot name
                }
            }
            else
            {
                Debug.LogError("Game JSON file is missing!");
            }
        }

        public void CheckMatches()
        {
            int matchedCount = 0;

            foreach (Transform slot in slotPanel.transform)
            {
                if (slot.childCount > 1)
                {
                    Transform draggableItem = slot.GetChild(1);
                    string draggedItemName = draggableItem.GetComponentInChildren<TextMeshProUGUI>().text;
                    string expectedPair = slot.name;

                    Debug.Log($"Checking slot: {slot.name}, Dragged item: {draggedItemName}");

                    if (pairs.ContainsKey(draggedItemName))
                    {
                        if (pairs[draggedItemName] == expectedPair)
                        {
                            Debug.Log($"Matched: {draggedItemName} -> {expectedPair}");
                            matchedCount++;
                        }
                        else
                        {
                            Debug.Log($"Mismatch: {draggedItemName} -> {expectedPair}");
                        }
                    }
                    else
                    {
                        Debug.Log($"No match found for dragged item: {draggedItemName}");
                    }
                }
                else
                {
                    Debug.Log($"Slot {slot.name} is empty or incomplete.");
                }
            }

            Debug.Log($"Matched pairs: {matchedCount}/{pairs.Count}");
        }
    }

    [System.Serializable]
    public class MatchData
    {
        public string challengeName;
        public List<Pair> pairs;
    }

    [System.Serializable]
    public class Pair
    {
        public string item;
        public string pair;
    }
}
