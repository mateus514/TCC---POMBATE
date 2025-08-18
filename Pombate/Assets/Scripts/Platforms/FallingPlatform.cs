using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float delayToFall = 0.5f;
    public float timeToReset = 2f;
    
    private TargetJoint2D target;
    private BoxCollider2D boxColl;
    private Vector2 startPos;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GetComponent<TargetJoint2D>();
        boxColl = GetComponent<BoxCollider2D>();
        startPos = transform.position;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Invoke("StartFalling", delayToFall);
        }
    }
    
    void StartFalling()
    {
        target.enabled = false;
        boxColl.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == 9)
        {
            gameObject.SetActive(false);
            Invoke("Resetar", timeToReset);
        }
    }

    

    void Resetar()
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        transform.position = startPos;
        gameObject.SetActive(true);
    }
}

