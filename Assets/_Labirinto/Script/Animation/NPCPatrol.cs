using UnityEngine;

public class NPCPatrol : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;
    private Rigidbody2D rb;
    private Animator anim;
    private Transform currentPoint;
    public float speed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentPoint = pointB.transform;
    }

    void Update()
    {
        // Move towards the current point
        Vector2 direction = currentPoint.position - transform.position;
        rb.velocity = new Vector2(direction.normalized.x * speed, 0);

        // Update animation
        anim.SetBool("IsWalking", rb.velocity != Vector2.zero);

        // Flip sprite based on direction
        if (direction.x > 0 && transform.localScale.x < 0 || direction.x < 0 && transform.localScale.x > 0)
        {
            flip();
        }

        // Switch points if close to current target
        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f)
        {
            currentPoint = currentPoint == pointB.transform ? pointA.transform : pointB.transform;
        }
    }

    private void flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void OnDrawGizmos()
    {
        if (pointA != null && pointB != null)
        {
            Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
            Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);
            Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
        }
    }
}
