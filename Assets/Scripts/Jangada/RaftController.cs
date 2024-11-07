using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class RaftController : MonoBehaviour
{
    public float velocidadeMaxima = 5.0f;
    public float aceleracao = 1.0f;
    public float inerciaRotacao = 0.95f;
    public float velocidadeRotacao = 2.0f;
    public float suavidadeRotacao = 0.1f;
    public float limiteRotacao = 30.0f;

    private bool estaMovendo = false;
    private Rigidbody rb;
    private Vector2 movimentoInput; // Armazena a entrada do joystick para movimento
    private float velocidadeAtual = 0.0f;
    private float velocidadeRotacaoAtual = 0.0f;
    public ParticleSystem rastroMovimento;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        AlternarMovimento(); // Ativa o movimento no início para facilitar o teste
    }

    private void FixedUpdate()
    {
        // Calcula a velocidade alvo com base no estado de movimento
        float velocidadeAlvo = estaMovendo ? velocidadeMaxima : 0.0f;
        velocidadeAtual = Mathf.Lerp(velocidadeAtual, velocidadeAlvo, aceleracao * Time.fixedDeltaTime);

        // Verifica se o input de movimento é maior que zero
        if (movimentoInput.magnitude > 0 && estaMovendo)
        {
            // Direção de movimento considerando -transform.right como frente
            Vector3 direcaoMovimento = (-transform.right * movimentoInput.y + transform.forward * movimentoInput.x).normalized;
            Vector3 movimento = direcaoMovimento * velocidadeAtual * Time.fixedDeltaTime;

            // Aplica a força para mover a jangada
            rb.AddForce(movimento, ForceMode.Acceleration);

            // Log de debug para confirmar o movimento
            Debug.Log("Movendo a jangada na direção: " + movimento);

            if (!rastroMovimento.isPlaying)
                rastroMovimento.Play();
        }
        else if (rastroMovimento.isPlaying)
        {
            rastroMovimento.Stop();
        }

        // Aplica a rotação suavemente, respeitando o input horizontal do joystick
        if (Mathf.Abs(movimentoInput.x) > 0.1f)
        {
            AplicarRotacao(movimentoInput.x);
        }

        // Aplica inércia à rotação
        if (Mathf.Abs(velocidadeRotacaoAtual) > 0.01f)
        {
            rb.angularVelocity = Vector3.ClampMagnitude(rb.angularVelocity, limiteRotacao);
            rb.AddTorque(Vector3.up * velocidadeRotacaoAtual, ForceMode.Acceleration);
            velocidadeRotacaoAtual *= inerciaRotacao;
        }
    }

    private void AplicarRotacao(float rotacaoInput)
    {
        float alvoRotacao = rotacaoInput * velocidadeRotacao;
        velocidadeRotacaoAtual = Mathf.Lerp(velocidadeRotacaoAtual, alvoRotacao, suavidadeRotacao);
        Debug.Log("Aplicando rotação: " + velocidadeRotacaoAtual);
    }

    public void AlternarMovimento()
    {
        estaMovendo = !estaMovendo;
        if (estaMovendo)
        {
            rastroMovimento.Play();
        }
        else
        {
            rastroMovimento.Stop();
        }
        Debug.Log("Movimento da jangada: " + (estaMovendo ? "Ligado" : "Desligado"));
    }

    // Método chamado pelo novo sistema de Input da Unity para capturar o movimento
    public void OnMove(InputAction.CallbackContext context)
    {
        movimentoInput = context.ReadValue<Vector2>();
        Debug.Log("Movimento Input: " + movimentoInput); // Log para confirmar que o input está chegando
    }
}
