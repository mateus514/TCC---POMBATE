using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetFase : MonoBehaviour
{
    void Update()
    {
        // Reinicia cena ao apertar R
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReiniciarCena();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Restart"))
        {
            ReiniciarCena();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Restart"))
        {
            ReiniciarCena();
        }
    }

    private void ReiniciarCena()
    {
        string cenaAtual = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(cenaAtual);
    }
}