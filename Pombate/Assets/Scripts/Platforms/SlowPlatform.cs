using UnityEngine;

public class SlowPlatform : MonoBehaviour
{
    public float multiplicadorVelocidade = 0.5f;
    public float multiplicadorPulo = 0.5f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.velocidade = Mathf.RoundToInt(player.velocidade * multiplicadorVelocidade);
                player.forcaPulo *= multiplicadorPulo;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.velocidade = Mathf.RoundToInt(player.velocidade / multiplicadorVelocidade);
                player.forcaPulo /= multiplicadorPulo;
            }
        }
    }
}