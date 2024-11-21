using TMPro;
using UnityEngine;

public class AccountInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerUIDText;
    [SerializeField] private TextMeshProUGUI playerNameText;

    private void Start()
    {
        playerUIDText.text = PlayerPrefs.GetString("Player UID");
        playerNameText.text = PlayerPrefs.GetString("PlayerName");
    }
}
