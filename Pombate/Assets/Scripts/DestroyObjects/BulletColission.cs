using UnityEngine;
using UnityEngine.Tilemaps;

public class BulletCollision : MonoBehaviour
{
    [Header("Detecção")]
    [SerializeField] private float destructionRadius = 0.1f; // Raio de busca para garantir detecção
    
    [Header("Som do bloco")]
    [SerializeField] private AudioClip somQuebraBloco; // arraste o som do bloco quebrando
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
                // Tenta múltiplos pontos para garantir a destruição
                Vector3 hitPosition = collision.contacts[0].point;
                
                // Converte para posição do tile
                Vector3Int cellPosition = tilemap.WorldToCell(hitPosition);
                
                bool tileDestruido = false;
                
                // Remove o tile principal
                if (tilemap.HasTile(cellPosition))
                {
                    tilemap.SetTile(cellPosition, null);
                    tileDestruido = true;
                }
                else
                {
                    // Se não achou, tenta as células ao redor
                    tileDestruido = DestroyNearbyTile(tilemap, hitPosition);
                }

                // 🔊 Toca som apenas se realmente destruiu um bloco
                if (tileDestruido && somDoJogador != null && somQuebraBloco != null)
                {
                    somDoJogador.TocarSomPersonalizado(somQuebraBloco);
                }
            }
        }

        // A bala sempre é destruída quando bate em algo
        Destroy(gameObject);
    }

    private bool DestroyNearbyTile(Tilemap tilemap, Vector3 worldPosition)
    {
        // Checa a célula central e as adjacentes
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector3 offset = new Vector3(x * destructionRadius, y * destructionRadius, 0);
                Vector3Int cellPos = tilemap.WorldToCell(worldPosition + offset);
                
                if (tilemap.HasTile(cellPos))
                {
                    tilemap.SetTile(cellPos, null);
                    return true; // Retorna true indicando que destruiu um tile
                }
            }
        }
        return false; // Não encontrou nenhum tile para destruir
    }
}