using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    private UnityAction onSelectAction;

    public void Initialize(Sprite itemSprite, UnityAction selectedAction)
    {
        if (itemImage != null)
        {
            itemImage.sprite = itemSprite;
            onSelectAction = selectedAction;

            // Check if the Image has a Button component
            Button itemButton = itemImage.GetComponent<Button>();
            if (itemButton != null)
            {
                itemButton.onClick.RemoveAllListeners();
                itemButton.onClick.AddListener(() => onSelectAction?.Invoke());
            }
            else
            {
                Debug.LogWarning("Button component is missing on the assigned itemImage GameObject.");
            }
        }
        else
        {
            Debug.LogWarning("ItemImage is not assigned in the inspector.");
        }
    }
}
