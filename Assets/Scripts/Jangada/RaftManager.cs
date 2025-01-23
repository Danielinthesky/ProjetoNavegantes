using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RaftManager : MonoBehaviour
{
    private Rigidbody rb;
    private Collider[] colliders;

    [Header("Configurações de Física")]
    public LayerMask camadasPersonagem; // Camadas do personagem
    public bool ignorarForcasDoPersonagem = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Configurações do Rigidbody para maior estabilidade
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        // Obtém todos os colliders no objeto e seus filhos
        colliders = GetComponentsInChildren<Collider>();
    }

    void OnCollisionStay(Collision collision)
    {
        // Verifica se o objeto na colisão é o personagem
        if (ignorarForcasDoPersonagem && (camadasPersonagem.value & (1 << collision.gameObject.layer)) != 0)
        {
            // Cancela as forças causadas pelo personagem
            CancelarForcasExternas();
        }
    }

    void FixedUpdate()
    {
        // Garante que o movimento residual seja suavizado
        SuavizarMovimento();
    }

    private void CancelarForcasExternas()
    {
        // Cancela as velocidades aplicadas ao Rigidbody
        rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, 0.1f); // Suaviza velocidade linear
        rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, Vector3.zero, 0.1f); // Suaviza velocidade angular
    }

    private void SuavizarMovimento()
    {
        // Suaviza o movimento residual (causado por forças externas)
        rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, 0.05f);
        rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, Vector3.zero, 0.05f);
    }
}
