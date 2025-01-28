using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovimento : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Configurações de Movimentação")]
    public float velocidadeMovimento = 5f;
    public float forcaPulo = 5f;
    public float multiplicadorCorrida = 1.5f;
    public float velocidadeRotacao = 10f;
    public float fatorForca = 0.5f;

    [Header("Configurações do Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Vector2 entradaMovimento;
    public bool EstaNoChao { get; private set; }
    public bool EstaMovendo { get; private set; }
    public bool EstaCorrendo { get; private set; }
    

    private bool estaCorrendo;
    public Vector3 NormalSuperficie { get; private set; } // Normal da superfície atual

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        VerificarGroundCheck();
        AtualizarMovimento();
        ManterNoChao();
    }

    private void VerificarGroundCheck()
{
    EstaNoChao = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

    if (!EstaNoChao)
    {
        // Aplica uma leve força descendente para ajudar a estabilizar o personagem
        rb.AddForce(Vector3.down * 10f, ForceMode.Acceleration);
    }
}


    private void AtualizarMovimento()
{
    // Direções baseadas na câmera
    Vector3 frenteCamera = Camera.main.transform.forward;
    frenteCamera.y = 0f;
    frenteCamera.Normalize();

    Vector3 direitaCamera = Camera.main.transform.right;
    direitaCamera.y = 0f;
    direitaCamera.Normalize();

    // Calcula a direção do movimento com base na entrada
    Vector3 direcaoMovimento = (frenteCamera * entradaMovimento.y + direitaCamera * entradaMovimento.x).normalized;

    EstaMovendo = direcaoMovimento.magnitude > 0.1f;
    EstaCorrendo = EstaMovendo && estaCorrendo;

    if (EstaMovendo)
    {
        // Define a velocidade final com base no estado de corrida
        float velocidadeFinal = EstaCorrendo ? velocidadeMovimento * multiplicadorCorrida : velocidadeMovimento;

        // Calcula a velocidade desejada (movimento horizontal)
        Vector3 velocidadeDesejada = new Vector3(direcaoMovimento.x * velocidadeFinal, rb.velocity.y, direcaoMovimento.z * velocidadeFinal);

        // Adiciona força vertical em inclinações
        if (EstaEmSuperficieIncline(NormalSuperficie))
        {
            velocidadeDesejada.y = Mathf.Max(0, rb.velocity.y + Mathf.Abs(Physics.gravity.y) * Time.fixedDeltaTime * fatorForca);
        }
        else
        {
            velocidadeDesejada.y = rb.velocity.y;
        }

        // Aplica a velocidade ao Rigidbody
        rb.velocity = velocidadeDesejada;

        // Rotaciona o personagem apenas com base no input do jogador
        // Rotação apenas no plano horizontal com base no input
    if (direcaoMovimento != Vector3.zero)
    {
        Quaternion rotacaoAlvo = Quaternion.LookRotation(new Vector3(direcaoMovimento.x, 0, direcaoMovimento.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacaoAlvo, velocidadeRotacao * Time.fixedDeltaTime);
    }

    }
    else
    {
        // Para o movimento suavemente se não houver entrada
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
        rb.angularVelocity = Vector3.zero;
    }

    if (EstaNoChao)
    {
        ManterNoChao();
    }
}







private bool EstaEmSuperficieIncline(Vector3 normal)
{
    // Calcula o ângulo entre a normal da superfície e o vetor "up"
    float angulo = Vector3.Angle(normal, Vector3.up);

    // Considere inclinações entre 0 e 45 graus (ou ajuste o valor)
    return angulo > 0f && angulo <= 55f;
}



    private void ManterNoChao()
    {
        if (EstaNoChao)
        {
            Vector3 forcaParaChao = Vector3.down * 10f;
            rb.AddForce(forcaParaChao, ForceMode.Acceleration);
        }
    }

    public void OnMover(Vector2 movimento)
    {
        entradaMovimento = movimento;
    }

    public void OnCorrer(bool correndo)
    {
        estaCorrendo = correndo;
    }

    private void OnDrawGizmos()
{
    if (groundCheck != null)
    {
        // Desenha o gizmo do groundCheck
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}


void OnCollisionStay(Collision collision)
{
    // Obtém a normal da superfície
    if (collision.contactCount > 0)
    {
        NormalSuperficie = collision.GetContact(0).normal;
    }
}

void OnCollisionExit(Collision collision)
{
    // Limpa a normal da superfície ao sair dela
    NormalSuperficie = Vector3.zero;
}

}
