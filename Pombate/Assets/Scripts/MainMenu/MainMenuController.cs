using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void JogarTutorial()
    {
        // Troca para a cena chamada "Tutorial1"
        SceneManager.LoadScene("Fase1 - Pule e Pule na borda");
    }
    public void SairDoJogo()
    {
        // Fecha o jogo
        Application.Quit();

        // Debug útil no editor
        Debug.Log("Sair do jogo chamado!");
    }
}
