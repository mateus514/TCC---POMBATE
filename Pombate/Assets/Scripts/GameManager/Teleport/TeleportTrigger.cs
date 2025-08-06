using UnityEngine;

public class TeleportTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<TeleportManager>().TeleportToTp2();
        }
    }
}