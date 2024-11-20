using UnityEngine;
using TMPro;

public class LeaderboardBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI medalText;
    [SerializeField] private TextMeshProUGUI rankText;

    public void Initialize(string playerName, string medal, string rank)
    {
        playerNameText.text = playerName;
        medalText.text = medal;
        rankText.text = rank;
    }
}
