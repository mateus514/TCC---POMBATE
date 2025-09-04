using System.Collections.Generic;
using UnityEngine;

public class TeleportManager : MonoBehaviour
{
    [System.Serializable]
    public class TeleportPair
    {
        public Transform entrada;
        public Transform saida;
    }

    [Header("Lista de pares de teleporte (entrada → saída)")]
    public List<TeleportPair> teleportPairs = new List<TeleportPair>();

    [Header("Referência ao jogador")]
    public Transform player;

    public void TeleportFrom(Transform entrada)
    {
        foreach (TeleportPair pair in teleportPairs)
        {
            if (pair.entrada == entrada && pair.saida != null)
            {
                player.position = pair.saida.position;

                if (player.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
                {
                    rb.linearVelocity = Vector2.zero;
                    rb.angularVelocity = 0f;
                }

                return;
            }
        }

        Debug.LogWarning("Ponto de entrada não encontrado nos pares de teleporte!");
    }
}