using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Newtonsoft.Json;
using Challenge;

namespace MatchChallenge
{
    public class MatchGameManager : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private GameObject matchPanel;
        [SerializeField] private Transform leftListParent; // For item1 (left side)
        [SerializeField] private Transform rightListParent; // For item2 (right side)
        [SerializeField] private GameObject draggableItemPrefab;

        private List<Pair> pairs;
        private Dictionary<string, string> pairMap;
        private int correctMatches = 0;

        void Start()
        {
            LoadMatchFromChallengeManager();
        }

        public void LoadMatchFromChallengeManager()
        {
            // Get the matchJson from the ChallengeManager
            TextAsset matchJson = ChallengeManager.Instance.dataJson;

            if (matchJson != null)
            {
                LoadMatch(matchJson);
            }
            else
            {
                Debug.LogError("No match JSON found in ChallengeManager!");
            }
        }

        public void LoadMatch(TextAsset matchJson)
        {
            if (matchJson != null)
            {
                string jsonText = matchJson.text;
                ChallengeData matchData = JsonConvert.DeserializeObject<ChallengeData>(jsonText);

                pairs = matchData.pairs;
                pairMap = new Dictionary<string, string>();

                foreach (var pair in pairs)
                {
                    pairMap[pair.item1] = pair.item2;
                    CreateDraggableItem(leftListParent, pair.item1);
                    CreateDraggableItem(rightListParent, pair.item2);
                }

                matchPanel.SetActive(true);
            }
            else
            {
                Debug.LogError("Match JSON file is missing!");
            }
        }

        void CreateDraggableItem(Transform parent, string text)
        {
            GameObject item = Instantiate(draggableItemPrefab, parent);
            item.GetComponentInChildren<TMP_Text>().text = text;
            item.name = text; // Set the name to identify the item
        }

        public void CheckMatch(GameObject item1, GameObject item2)
        {
            // Check if the items are in the correct pair
            if (pairMap.TryGetValue(item1.name, out string correctPair))
            {
                if (item2.name == correctPair)
                {
                    // Correct match
                    Debug.Log($"Matched: {item1.name} with {item2.name}");
                    correctMatches++;
                    item1.SetActive(false); // Optionally hide matched items
                    item2.SetActive(false);
                }
                else
                {
                    // Incorrect match
                    Debug.Log($"Wrong match: {item1.name} and {item2.name}");
                }
            }

            // Check if all matches are made
            if (correctMatches == pairs.Count)
            {
                EndMatch();
            }
        }

        void EndMatch()
        {
            Debug.Log("All pairs matched! Challenge Complete!");
            matchPanel.SetActive(false); // Hide the match panel
        }
    }

    [System.Serializable]
    public class Pair
    {
        public string item1;
        public string item2;
    }

    [System.Serializable]
    public class ChallengeData
    {
        public string challengeName;
        public List<Pair> pairs;
    }
}
