using UnityEngine;

public class Credits : MonoBehaviour
{
    public float scrollSpeed = 30f;
    public float startingYPosition = -10f;
    public float endYPosition = 10f;

    private RectTransform textRectTransform;

    void Start()
    {
        textRectTransform = GetComponent<RectTransform>();
        textRectTransform.anchoredPosition = new Vector2(0, startingYPosition);
    }

    void Update()
    {
        // Scroll the text upwards
        textRectTransform.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);

        // Once the text scrolls past the screen, reset it to the start position for a looping effect (optional)
        if (textRectTransform.anchoredPosition.y > endYPosition)
        {
            textRectTransform.anchoredPosition = new Vector2(0, startingYPosition);
        }
    }
}
