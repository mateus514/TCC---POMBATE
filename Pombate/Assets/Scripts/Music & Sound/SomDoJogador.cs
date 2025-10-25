using UnityEngine;

public class SomDoJogador : MonoBehaviour
{
    public UnityEngine.AudioSource audioSource; // ⬅️ tipo completo
    public AudioClip somPulo;
    public AudioClip somTiro;
    public AudioClip somMorte;

    public void TocarSomPulo()
    {
        if (audioSource != null && somPulo != null)
            audioSource.PlayOneShot(somPulo);
    }

    public void TocarSomTiro()
    {
        if (audioSource != null && somTiro != null)
            audioSource.PlayOneShot(somTiro);
    }
    public void TocarSomMorte()
    {
        if (audioSource != null && somMorte != null)
            audioSource.PlayOneShot(somMorte);
    }
}