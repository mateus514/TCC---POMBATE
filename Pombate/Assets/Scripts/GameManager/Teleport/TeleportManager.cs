using UnityEngine;

public class TeleportManager : MonoBehaviour
{
    [Header("Pontos de teleporte")]
    public Transform tp1;
    public Transform tp2;

    [Header("Referência ao jogador")]
    public Transform player;

    public void TeleportToTp2()
    {
        if (player != null && tp2 != null)
        {
            player.position = tp2.position;
        }
        else
        {
            Debug.LogWarning("Player ou tp2 não atribuído!");
        }
    }
}
