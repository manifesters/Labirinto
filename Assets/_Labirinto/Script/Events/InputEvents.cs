using UnityEngine;
using System;

public class InputEvents
{
    public event Action onSubmitPressed;
    public void SubmitPressed()
    {
        onSubmitPressed?.Invoke();
    }
    public InputEvents()
    {
        Debug.Log("InputEvents initialized");
    }
}