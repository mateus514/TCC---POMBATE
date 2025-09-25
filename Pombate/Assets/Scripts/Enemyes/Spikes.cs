using System;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    public Player player; // Use o script correto do seu player
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
            // Reseta posição e rotação
            player.transform.position = playerStartPos;
            player.transform.rotation = playerStartRot;

            // Reseta velocidade e estado do Rigidbody
            player.ResetarEstado();
        }
    }
}