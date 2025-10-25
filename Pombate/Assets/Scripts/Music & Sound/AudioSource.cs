using UnityEngine;

public class AudioSource : MonoBehaviour
{
    private static AudioSource instancia;

    void Awake()
    {
        // Se já existir uma instância, destrói a nova para não duplicar a música
        if (instancia != null && instancia != this)
        {
            Destroy(gameObject);
            return;
        }

        instancia = this;
        DontDestroyOnLoad(gameObject); // Impede que o objeto seja destruído ao trocar de cena
    }
}