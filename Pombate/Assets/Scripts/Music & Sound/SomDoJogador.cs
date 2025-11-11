using UnityEngine;

public class SomDoJogador : MonoBehaviour
{
    [Header("Audio Sources")]
    public UnityEngine.AudioSource audioSourcePassos; // arraste aqui o AudioSource de passos
    public UnityEngine.AudioSource audioSourceEfeitos; // para tiros, pulo, morte etc.

    [Header("Sons")]
    public AudioClip somPulo;
    public AudioClip somTiro;
    public AudioClip somMorte;

    // ----------------------------
    // Métodos para cada som
    // ----------------------------
    public void TocarSomPulo()
    {
        audioSourceEfeitos.PlayOneShot(somPulo);
    }

    public void TocarSomTiro()
    {
        audioSourceEfeitos.PlayOneShot(somTiro);
    }

    public void TocarSomMorte()
    {
        audioSourceEfeitos.PlayOneShot(somMorte);
    }

    // ----------------------------
    // Passos
    // ----------------------------
    public void TocarSomPersonalizado(AudioClip clip)
    {
        if (audioSourceEfeitos != null && clip != null)
            audioSourceEfeitos.PlayOneShot(clip);
    }

}