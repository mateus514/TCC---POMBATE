using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ProximaFase : MonoBehaviour
{
    [Header("Fade")]
    public CanvasGroup telaPreta; // atribua a imagem preta com CanvasGroup
    public float duracaoFade = 1f;

    public void CarregarCena(string nomeCena)
    {
        StartCoroutine(FadeECarregar(nomeCena));
    }

    private IEnumerator FadeECarregar(string nomeCena)
    {
        // Fade para preto
        float t = 0;
        while (t < duracaoFade)
        {
            t += Time.deltaTime;
            telaPreta.alpha = t / duracaoFade;
            yield return null;
        }
        telaPreta.alpha = 1;

        // Carregar cena
        SceneManager.LoadScene(nomeCena);
    }
}