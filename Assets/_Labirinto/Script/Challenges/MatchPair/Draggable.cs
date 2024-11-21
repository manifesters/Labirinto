using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    // Reference to the target DropZone (can be the 'left' or 'right' zone)
    private Dropzone dropzone;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();

        dropzone = GetComponentInParent<Dropzone>(); // Find DropZone parent
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Mobile drag - hide the item slightly for visual feedback
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Move the item according to touch movement (scaled by the canvas scale factor)
        Vector2 touchPosition = eventData.position;
        rectTransform.position = touchPosition / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Reset alpha and enable raycasting
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        // After dragging, call the drop zone to check if the item is in the correct position
        if (dropzone != null)
        {
            dropzone.OnItemDropped(this.gameObject);
        }
    }
}
