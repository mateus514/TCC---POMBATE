using UnityEngine;

public class SlowPlatform : MonoBehaviour
{
    public float multiplicadorVelocidade = 0.5f;
    public float multiplicadorPulo = 0.5f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMove player = collision.gameObject.GetComponent<PlayerMove>();
            if (player != null)
            {
                player.speed *= multiplicadorVelocidade;
                player.jumpForce *= multiplicadorPulo;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMove player = collision.gameObject.GetComponent<PlayerMove>();
            if (player != null)
            {
                player.speed /= multiplicadorVelocidade;
                player.jumpForce /= multiplicadorPulo;
            }
        }
    }
}