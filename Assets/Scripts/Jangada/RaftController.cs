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

    private Rigidbody rb;
    private Vector2 movimentoInput; // Armazena a entrada do joystick para movimento
    private float velocidadeAtual = 0.0f;
    private float velocidadeRotacaoAtual = 0.0f;
    public bool jangadaEmMovimento = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (jangadaEmMovimento)
        {
            // Move a jangada automaticamente para frente
            Vector3 movimento = transform.forward * velocidadeMaxima * Time.fixedDeltaTime;
            rb.AddForce(movimento, ForceMode.Acceleration);
        }

        // Rotaciona a jangada com base no input
        if (movimentoInput.magnitude > 0)
        {
            AplicarRotacao(movimentoInput.x);
        }
    }
    private void AplicarRotacao(float rotacaoInput)
    {
        float torque = rotacaoInput * velocidadeRotacao;
        rb.AddTorque(Vector3.up * torque, ForceMode.Acceleration);
    }

    // Método chamado pelo novo sistema de Input da Unity para capturar o movimento
    public void OnMove(InputAction.CallbackContext context)
    {
        movimentoInput = context.ReadValue<Vector2>();
        Debug.Log("Movimento Input: " + movimentoInput); // Log para confirmar que o input está chegando
    }
    public void Mover(Vector2 direcao)
{
    // Lógica para movimentar a jangada com base na entrada
    Debug.Log($"Movendo jangada na direção: {direcao}");
}

    public void ToggleMovement()
    {
        jangadaEmMovimento = !jangadaEmMovimento;
        Debug.Log("Jangada em movimento: " + jangadaEmMovimento);
    }
}
