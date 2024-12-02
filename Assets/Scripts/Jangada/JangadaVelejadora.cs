using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class JangadaVelejadora : MonoBehaviour
{
    public float velocidadeMaxima = 5.0f;
    public float aceleracao = 1.0f;
    public Transform vela; // Referência para o objeto da vela
    public bool estaAncorada = true; // Indica se a jangada está ancorada
    public float intensidadeVento = 1.0f; // Intensidade do vento que afeta a velocidade da jangada
    public float anguloEficiente = 30f; // Ângulo ideal da vela em relação ao movimento
    private Rigidbody rb;
    public float velocidadeRotacao = 100f;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!estaAncorada)
        {
            MoverJangadaComVento();
        }
        else
        {
            // Parar a jangada se estiver ancorada
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    private void MoverJangadaComVento()
    {
        // Calcula a direção do vento com base na rotação da vela, usando -transform.right como frente
        Vector3 direcaoVento = -vela.right;

        // Calcula o ângulo entre a frente da jangada (-transform.right) e a direção do vento (orientação da vela)
        float angulo = Vector3.Angle(-transform.right, direcaoVento);

        // Aplica uma penalidade na velocidade caso a vela não esteja na posição eficiente
        float fatorEficiencia = Mathf.Clamp01(1.0f - Mathf.Abs(angulo - anguloEficiente) / anguloEficiente);

        // Calcula o movimento com base na intensidade do vento e a eficiência da orientação da vela
        Vector3 movimento = direcaoVento * velocidadeMaxima * intensidadeVento * fatorEficiencia * Time.fixedDeltaTime;

        // Aplica um leve efeito de derrapagem para simular a correnteza
        Vector3 derrapagem = Vector3.Cross(direcaoVento, Vector3.up) * 0.1f;
        movimento += derrapagem;

        // Aplica a força para mover a jangada na direção do vento
        rb.AddForce(movimento, ForceMode.Acceleration);
        
        // Log de debug para ver a direção, ângulo e a velocidade
        Debug.Log("Movendo a jangada com eficiência: " + fatorEficiencia + " na direção do vento: " + movimento);
    }

    // Método para alternar o estado de ancoragem
    public void AlternarAncoragem()
    {
        estaAncorada = !estaAncorada;
        Debug.Log("Jangada ancorada: " + estaAncorada);
    }

    // Método para rodar a vela baseado em um input, por exemplo, do joystick direito
    public void OnRotateSail(InputAction.CallbackContext context)
    {
        float rotacaoInput = context.ReadValue<float>();
        vela.Rotate(0, rotacaoInput, 0); // Rotaciona a vela no eixo Y
    }

    public void DefinirEntradaJoystick(float valorRotacao)
    {
        float rotacao = valorRotacao * velocidadeRotacao * Time.deltaTime;
        transform.Rotate(0, rotacao, 0); // Aplica a rotação na jangada
    }
}
