using UnityEngine;
using UnityEngine.Tilemaps;

public class BulletCollision : MonoBehaviour
{
    [Header("Som do bloco")]
    public AudioClip somQuebraBloco; // arraste o som do bloco quebrando no SomDoJogador
    private SomDoJogador somDoJogador;

    void Start()
    {
        // Busca automaticamente o SomDoJogador na cena
        if (somDoJogador == null)
        {
            somDoJogador = FindObjectOfType<SomDoJogador>();
            if (somDoJogador == null)
            {
                Debug.LogWarning("SomDoJogador não encontrado na cena!");
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Só reage se o objeto tiver a tag "destruir"
        if (collision.collider.CompareTag("destruir"))
        {
            Tilemap tilemap = collision.collider.GetComponent<Tilemap>();
            if (tilemap != null)
            {
                // Pega a posição exata onde a bala bateu
                Vector3 hitPosition = Vector3.zero;
                foreach (ContactPoint2D hit in collision.contacts)
                {
                    hitPosition = hit.point;
                }

                // Converte a posição do impacto para célula do tilemap
                Vector3Int cellPosition = tilemap.WorldToCell(hitPosition);

                // Remove só aquele tile atingido
                tilemap.SetTile(cellPosition, null);

                // 🔊 Toca som do bloco quebrando
                if (somDoJogador != null && somQuebraBloco != null)
                {
                    somDoJogador.TocarSomPersonalizado(somQuebraBloco);
                }
            }
        }

        // A bala sempre é destruída quando bate em algo
        Destroy(gameObject);
    }
}