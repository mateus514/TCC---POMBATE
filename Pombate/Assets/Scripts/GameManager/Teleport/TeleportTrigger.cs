using UnityEngine;

public class TeleportTrigger : MonoBehaviour
{
    [Header("Tag do player")]
    public string playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            TeleportManager manager = FindObjectOfType<TeleportManager>();
            if (manager != null)
            {
                manager.TeleportFrom(transform);
            }
        }
    }
}