using UnityEngine;
using UnityEngine.AI;

public class CompanionIA : MonoBehaviour
{
    public Transform alvo; // Transform do personagem controlado pelo jogador
    public float stopDistance = 2f; // Distância para parar próximo ao alvo
    public float followDistance = 5f; // Distância para começar a seguir o alvo

    private NavMeshAgent agente;

    void Awake()
    {
        agente = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (alvo == null) return;

        float distancia = Vector3.Distance(transform.position, alvo.position);

        if (distancia > followDistance)
        {
            // Iniciar movimento quando a distância for maior que followDistance
            agente.SetDestination(alvo.position);
        }
        else if (distancia <= stopDistance)
        {
            // Parar o movimento quando dentro de stopDistance
            agente.ResetPath();
        }
        else
        {
            // Se entre followDistance e stopDistance, manter posição
            agente.ResetPath();
        }
    }
}
