using UnityEngine;

public class ElevatorPlatform : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float velocidade = 2f;

    private Vector3 target;

    void Start()
    {
        // Começa indo para o ponto B
        target = pointB.position;
    }

    void Update()
    {
        // Move rigidamente ao longo da reta entre A e B
        transform.position = Vector3.MoveTowards(transform.position, target, velocidade * Time.deltaTime);

        // Quando chega bem perto do destino, troca o alvo
        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            target = (target == pointA.position) ? pointB.position : pointA.position;
        }

        // Corrige qualquer desvio (mantém sempre no plano da reta)
        Vector3 direction = (pointB.position - pointA.position).normalized;
        float projectedDistance = Vector3.Dot(transform.position - pointA.position, direction);
        transform.position = pointA.position + direction * projectedDistance;
    }
}