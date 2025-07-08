using UnityEngine;

public class MainMenuParallax : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float offsetMultiplier = 1f;
    public float smoothTime = 0.3f; 
    
    private Vector2 startPosition;
    private Vector3 velocity;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        Vector2 offset = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = Vector3.SmoothDamp(transform.position, startPosition + (offset * offsetMultiplier), ref velocity, smoothTime);
    }
}
