using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CircleCollider2D))]
public class CollectibleItemPoint : MonoBehaviour
{
    [Header("Item Info")]
    [SerializeField] private CollectibleItemSO itemInfoForPoint;

    [Header("Config")]
    [SerializeField] private bool collectable = true;

    [Header("Collect Button")]
    [SerializeField] private Button collectButton;
    [SerializeField] private Canvas mainCanvas;

    private bool playerIsNear = false;
    private RectTransform buttonRectTransform;
    private CollectibleManager collectibleManager;

    private void Awake()
    {
        // Find the CollectibleManager instance in the scene
        collectibleManager = FindObjectOfType<CollectibleManager>();

        // Ensure the button is hidden at the start
        collectButton.gameObject.SetActive(false);
        buttonRectTransform = collectButton.GetComponent<RectTransform>();

        // Add listener to the collect button click event
        collectButton.onClick.AddListener(OnCollectButtonClicked);
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.CompareTag("Player"))
        {
            playerIsNear = true;
            UpdateButtonVisibility();
        }
    }

    private void OnTriggerExit2D(Collider2D otherCollider)
    {
        if (otherCollider.CompareTag("Player"))
        {
            playerIsNear = false;
            collectButton.gameObject.SetActive(false);

            // Delay setting the parent to avoid Canvas deactivation issue
            if (mainCanvas.isActiveAndEnabled) 
            {
                buttonRectTransform.SetParent(this.transform);
            }
        }
    }

    private void Start()
    {
        // Check if the item has been collected from CollectibleManager and set its collectable state
        if (collectibleManager != null && collectibleManager.IsItemCollected(itemInfoForPoint.id))
        {
            gameObject.SetActive(false);
            collectable = false;
            collectButton.gameObject.SetActive(false);
        }
    }

    private void UpdateButtonVisibility()
    {
        if (playerIsNear && collectable)
        {
            // Only change the parent if the main canvas is active
            if (mainCanvas.isActiveAndEnabled && collectButton.transform.parent != mainCanvas.transform)
            {
                buttonRectTransform.SetParent(mainCanvas.transform, false);
                buttonRectTransform.anchoredPosition = new Vector2(300, -70);
            }

            collectButton.gameObject.SetActive(true);
        }
        else
        {
            collectButton.gameObject.SetActive(false);
        }
    }

    private void OnCollectButtonClicked()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.button_click);
        if (playerIsNear && collectable && collectibleManager != null)
        {
            collectibleManager.CollectItem(itemInfoForPoint.id);
            CollectItem();
        }
    }

    private void CollectItem()
    {
        // Log and disable the collect button
        Debug.Log("Item collected locally: " + itemInfoForPoint.id);
        collectButton.gameObject.SetActive(false);
        collectable = false;
        gameObject.SetActive(false);
    }
}
