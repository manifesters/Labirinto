using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class DraggableWord : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector2 originalPosition;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;
    private TextMeshProUGUI textComponent;
    private Color originalColor;

    void Awake()
    {
       //  Retrieves the component
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
        textComponent = GetComponent<TextMeshProUGUI>();
        originalColor = textComponent.color;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Saves the original position before dragging starts
        originalPosition = rectTransform.anchoredPosition;
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Moves the object based on the pointer's movement
        if (canvas != null)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        // Add a check to see if the word was dropped in a valid drop zone
        if (!eventData.pointerCurrentRaycast.isValid || eventData.pointerCurrentRaycast.gameObject.GetComponent<DropZone>() == null)
        {
            ResetPosition();
        }
    }

    public void ResetPosition()
    {
        // Resets the object's position and color
        rectTransform.anchoredPosition = originalPosition;
        textComponent.color = originalColor;
    }

    public void SetColor(Color color)
    {
        // Changes the color
        textComponent.color = color;
    }
}
