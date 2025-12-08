using UnityEngine;

public class Spikes : MonoBehaviour
{
    public Player player; // continua igual
    public SpikesData data; // <-- ScriptableObject

    private Vector3 playerStartPos;
    private Quaternion playerStartRot;

    void Start()
    {
        playerStartPos = player.transform.position;
        playerStartRot = player.transform.rotation;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (data.tocarSomDeMorte && player.somDoJogador != null)
            player.somDoJogador.TocarSomMorte();

        // aplica dano (se quiser usar isso depois)
        //player.ReceberDano(data.dano);//

        if (data.resetaPosicao)
        {
            player.transform.position = playerStartPos;
            player.transform.rotation = playerStartRot;
            player.ResetarEstado();
        }
    }
}