using UnityEngine;

public class WeaponCollider : MonoBehaviour
{
    [Header("Som da arma")]
    public UnityEngine.AudioSource audioSource;   // Arraste o AudioSource da arma aqui
    public AudioClip somBatida;       // Som do metal batendo no chão

    [Header("Layer do chão")]
    public string chaoLayerName = "Chao";  // Nome do layer do chão

    private bool tocouChao = false;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!tocouChao && collision.gameObject.layer == LayerMask.NameToLayer(chaoLayerName))
        {
            tocouChao = true;
            if (audioSource != null && somBatida != null)
                audioSource.PlayOneShot(somBatida);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Quando a arma sair do chão, permite tocar som novamente
        if (collision.gameObject.layer == LayerMask.NameToLayer(chaoLayerName))
        {
            tocouChao = false;
        }
    }
}