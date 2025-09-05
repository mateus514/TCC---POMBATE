using UnityEngine;

public class Spikes : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.CompareTag("Player"))
        {
            collision.GetComponentInParent<Player>().Morrer();
        }
    }
}
