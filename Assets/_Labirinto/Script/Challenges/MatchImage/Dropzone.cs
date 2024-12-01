using UnityEngine;
using UnityEngine.EventSystems;

public class Dropzone : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            Debug.Log($"Item {eventData.pointerDrag.name} dropped on {gameObject.name}");
            eventData.pointerDrag.transform.SetParent(transform);
            eventData.pointerDrag.transform.localPosition = Vector3.zero;
        }
        else
        {
            Debug.Log($"No item dropped on {gameObject.name}");
        }
    }
}
