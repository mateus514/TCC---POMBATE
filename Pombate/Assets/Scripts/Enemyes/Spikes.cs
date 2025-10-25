using UnityEngine;

public class Spikes : MonoBehaviour
{
    public Player player; // Arraste o Player no Inspector
    private Vector3 playerStartPos;
    private Quaternion playerStartRot;

    void Start()
    {
        // Salva a posição inicial do player no início da cena
        playerStartPos = player.transform.position;
        playerStartRot = player.transform.rotation;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Toca o som de morte/dano
            if (player.somDoJogador != null)
                player.somDoJogador.TocarSomMorte();

            // Reseta posição e rotação manualmente
            player.transform.position = playerStartPos;
            player.transform.rotation = playerStartRot;

            // Reseta velocidade e estado do Rigidbody
            player.ResetarEstado();
        }
    }
}
