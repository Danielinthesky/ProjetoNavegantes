using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RaftController : MonoBehaviour
{
    public float velocidadeMaxima = 5.0f;
    public float aceleracao = 1.0f;
    public float inerciaRotacao = 0.95f;
    public float velocidadeRotacao = 2.0f;      // Velocidade de rotação da jangada
    public float suavidadeRotacao = 0.1f;       // Controla a suavidade da rotação
    public float limiteRotacao = 30.0f;         // Limite da velocidade angular
    private bool estaMovendo = false;
    private Rigidbody rb;
    private float velocidadeAtual = 0.0f;
    private float velocidadeRotacaoAtual = 0.0f;
    public ParticleSystem dust;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Movimento linear da jangada
        float velocidadeAlvo = estaMovendo ? velocidadeMaxima : 0.0f;
        velocidadeAtual = Mathf.Lerp(velocidadeAtual, velocidadeAlvo, aceleracao * Time.deltaTime);

        if (estaMovendo)
        {
            rb.AddForce(-transform.right * velocidadeAtual, ForceMode.Acceleration);
            if (!dust.isPlaying) dust.Play();
        }
        else if (dust.isPlaying)
        {
            dust.Stop();
        }

        // Aplica rotação com inércia e limita a velocidade angular
        if (Mathf.Abs(velocidadeRotacaoAtual) > 0.01f)
        {
            // Limita a velocidade angular do Rigidbody diretamente
            rb.angularVelocity = Vector3.ClampMagnitude(rb.angularVelocity, limiteRotacao);
            rb.AddTorque(Vector3.up * velocidadeRotacaoAtual, ForceMode.Acceleration);
            velocidadeRotacaoAtual *= inerciaRotacao;
        }
    }

    // Método para aplicar rotação suavemente à jangada com limite
    public void AplicarRotacao(float rotacaoInput)
    {
        float alvoRotacao = rotacaoInput * velocidadeRotacao;
        velocidadeRotacaoAtual = Mathf.Lerp(velocidadeRotacaoAtual, alvoRotacao, suavidadeRotacao);
    }

    public void AlternarMovimento()
    {
        estaMovendo = !estaMovendo;
        if (estaMovendo) dust.Play(); else dust.Stop();
        Debug.Log("Movimento da jangada: " + (estaMovendo ? "Ligado" : "Desligado"));
    }
}
