using UnityEngine;

public class TriggerProximaFase : MonoBehaviour
{
    [Header("Cena a carregar")]
    public string nomeDaCena;

    private ProximaFase proximaFase;

    private void Start()
    {
        proximaFase = FindObjectOfType<ProximaFase>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && proximaFase != null)
        {
            proximaFase.CarregarCena(nomeDaCena);
        }
    }
}