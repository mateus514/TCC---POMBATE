using UnityEngine;

public class TriggerProximaFase : MonoBehaviour
{
    [Header("Cena a carregar")]
    public string nomeDaCena;
    
    [Header("Configuração de Fase")]
    [Tooltip("Índice desta fase (0 = primeira, 1 = segunda, etc)")]
    public int indiceAtualFase = 0;
    
    [Tooltip("Índice da próxima fase que será desbloqueada")]
    public int proximaFaseDesbloquear = 1;

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

            // SALVA O PROGRESSO
            SaveSystem.Instance.SaveGame(proximaFaseDesbloquear, nomeDaCena);
            
            Debug.Log($"Fase {indiceAtualFase} completada! Desbloqueada: {proximaFaseDesbloquear}");

            if (proximaFase != null)
            {
                proximaFase.CarregarCena(nomeDaCena);
            }
        }
    }
}