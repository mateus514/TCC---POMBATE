using UnityEngine;

public class TriggerProximaFase : MonoBehaviour
{
    [Header("Cena a carregar")]
    public string nomeDaCena;

    private ProximaFase proximaFase;
    private TimeBar timerBar;

    private void Start()
    {
        proximaFase = FindObjectOfType<ProximaFase>();
        timerBar = FindObjectOfType<TimeBar>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (timerBar != null)
            {
                timerBar.StopTimer();
            }

            if (proximaFase != null)
            {
                proximaFase.CarregarCena(nomeDaCena);
            }
        }
    }
}
