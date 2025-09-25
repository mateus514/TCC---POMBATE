using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    [Header("Referências")]
    public TimeBar timerBar;
    public CanvasGroup gameOverCanvasGroup;

    [Header("Configurações")]
    public float fadeDuration = 1.5f;

    public bool isGameOver { get; private set; } = false;

    void Start()
    {
        if (timerBar != null)
            timerBar.OnTimerEnd += HandleGameOver;

        if (gameOverCanvasGroup != null)
        {
            gameOverCanvasGroup.alpha = 0f;
            gameOverCanvasGroup.gameObject.SetActive(false);
        }
    }

    private void HandleGameOver()
    {
        StartCoroutine(GameOverSequence());
    }

    private IEnumerator GameOverSequence()
    {
        // Pausar o jogo
        Time.timeScale = 0f;

        // Ativar tela de Game Over
        gameOverCanvasGroup.gameObject.SetActive(true);

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            gameOverCanvasGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }

        gameOverCanvasGroup.alpha = 1f;

        // Marca que o jogo está em game over
        isGameOver = true;
    }

    void Update()
    {
        // Se estiver em game over e apertar qualquer tecla
        if (isGameOver && Input.anyKeyDown)
        {
            ReiniciarCena();
        }
    }

    private void ReiniciarCena()
    {
        // Volta o tempo ao normal antes de reiniciar
        Time.timeScale = 1f;

        string cenaAtual = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(cenaAtual);
    }
}