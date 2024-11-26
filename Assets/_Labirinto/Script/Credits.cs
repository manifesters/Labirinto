using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    public float scrollSpeed = 30f;  // Speed at which the credits scroll
    public float startingYPosition = -10f;  // Starting Y position of the text
    public float endYPosition = 10f;  // Ending Y position of the text

    private RectTransform textRectTransform;

    void Start()
    {
        textRectTransform = GetComponent<RectTransform>();
        textRectTransform.anchoredPosition = new Vector2(0, startingYPosition);  // Start the text offscreen
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
