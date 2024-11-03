using UnityEngine;

public class OceanFollow : MonoBehaviour
{
    public Transform raft; // A referência para o objeto da jangada
    public float oceanHeight = 0f; // Altura fixa do oceano

    void Update()
    {
        if (raft == null)
        {
            Debug.LogError("Raft Transform is not assigned to OceanFollow on " + gameObject.name);
            return;
        }

        // Atualiza a posição do oceano para a posição da jangada, mantendo a altura constante
        transform.position = new Vector3(raft.position.x, oceanHeight, raft.position.z);
    }
}
