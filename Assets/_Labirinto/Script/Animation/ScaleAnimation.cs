using UnityEngine;

public class ScaleAnimation : MonoBehaviour
{
    public float scaleFactor = 1.2f;
    public float speed = 2.0f;

    private Vector3 originalScale;
    private float timer;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        timer += Time.deltaTime * speed;
        float scale = Mathf.Lerp(1f, scaleFactor, Mathf.PingPong(timer, 1f));
        transform.localScale = originalScale * scale;
    }
}
