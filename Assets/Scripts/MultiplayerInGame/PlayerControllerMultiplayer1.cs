using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using Cinemachine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControllerMultiplayer1 : MonoBehaviourPun
{
    [Header("Configurações de Velocidade")]
    public float velocidadeBase = 5f;
    public float incrementoVelocidade = 2f;
    public float velocidadeMaxima = 10f;

    [Header("Referências")]
    private Rigidbody rb;
    private Animator animador;
    private Camera mainCamera;
    private WaterFloatMultiplayer waterFloatMultiplayer;
    private StaminaPersonagemMultiplayer staminaManagerMultiplayer;

    private Vector2 inputMovimento;
    private Vector3 direcaoMovimento;
    private float velocidadeAtual;
    private bool usandoRigidbody;

    private CinemachineFreeLook cinemachineCam;

    public enum EstadoPersonagem
    {
        Idle,
        Walking,
        Running,
        IdleSwimming,
        Swimming
    }

    private EstadoPersonagem estadoAtual = EstadoPersonagem.Idle;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animador = GetComponent<Animator>();

        // Valida se os componentes essenciais estão presentes
        if (animador == null)
        {
            Debug.LogError("Animator não encontrado no GameObject!", this);
        }
        if (rb == null)
        {
            Debug.LogError("Rigidbody não encontrado no GameObject!", this);
        }
    }

    private IEnumerator Start()
    {
        // Configuração da Cinemachine Free Look
        cinemachineCam = FindObjectOfType<CinemachineFreeLook>();
        if (cinemachineCam != null)
        {
            cinemachineCam.Follow = transform;
            cinemachineCam.LookAt = transform;
            cinemachineCam.m_XAxis.Value = 0f; // Ajusta ângulo inicial horizontal
            cinemachineCam.m_YAxis.Value = 0.5f; // Ajusta ângulo inicial vertical
        }
        else
        {
            Debug.LogError("Cinemachine Free Look não foi encontrada na cena.", this);
        }

        // Aguarda até que os componentes estejam carregados
        yield return GameInitializer.Instance.InicializarComponentes(gameObject);

        // Verifique os componentes após a inicialização
        staminaManagerMultiplayer = GetComponent<StaminaPersonagemMultiplayer>();
        if (staminaManagerMultiplayer == null)
        {
            Debug.LogError("StaminaPersonagemMultiplayer ainda não encontrado após inicialização!", this);
        }
        else
        {
            Debug.Log("StaminaPersonagemMultiplayer carregado com sucesso.", this);
        }

        if (!photonView.IsMine)
        {
            rb.isKinematic = true; // Desativa física para jogadores remotos
        }
        else
        {
            rb.isKinematic = false; // Ativa física para o jogador local
        }
    }

    private void Update()
    {
        if (!photonView.IsMine) return; // Apenas o jogador local controla o movimento

        AtualizarEstado();

        if (usandoRigidbody)
        {
            MoverNaAgua();
        }
        else
        {
            MoverPersonagem();
        }

        // Reduz a velocidade gradualmente
        if (velocidadeAtual > velocidadeBase)
        {
            velocidadeAtual -= incrementoVelocidade * Time.deltaTime;
            if (velocidadeAtual < velocidadeBase)
            {
                velocidadeAtual = velocidadeBase;
            }
        }
    }

    public void OnMover(InputAction.CallbackContext context)
    {
        if (!photonView.IsMine) return;

        inputMovimento = context.ReadValue<Vector2>();
    }

    public void OnToqueNoPainel()
    {
        if (!photonView.IsMine) return;

        if (!usandoRigidbody && staminaManagerMultiplayer.ConsumirStamina())
        {
            velocidadeAtual = Mathf.Min(velocidadeAtual + incrementoVelocidade, velocidadeMaxima);
        }
    }

    private void AtualizarEstado()
    {
        bool naAgua = waterFloatMultiplayer != null && waterFloatMultiplayer.EstaNaAgua;
        bool movendo = inputMovimento.magnitude > 0.1f;

        if (naAgua)
        {
            if (movendo)
                TrocarEstado(EstadoPersonagem.Swimming);
            else
                TrocarEstado(EstadoPersonagem.IdleSwimming);
        }
        else
        {
            if (movendo)
                TrocarEstado(velocidadeAtual > velocidadeBase ? EstadoPersonagem.Running : EstadoPersonagem.Walking);
            else
                TrocarEstado(EstadoPersonagem.Idle);
        }
    }

    private void TrocarEstado(EstadoPersonagem novoEstado)
    {
        if (estadoAtual == novoEstado) return;

        estadoAtual = novoEstado;

        if (animador == null)
        {
            Debug.LogError("Animator está ausente. Não é possível trocar o estado!", this);
            return;
        }

        animador.SetBool("isIdle", false);
        animador.SetBool("isWalking", false);
        animador.SetBool("isRunning", false);
        animador.SetBool("isIdleSwimming", false);
        animador.SetBool("isSwimming", false);

        switch (estadoAtual)
        {
            case EstadoPersonagem.Idle:
                animador.SetBool("isIdle", true);
                break;
            case EstadoPersonagem.Walking:
                animador.SetBool("isWalking", true);
                break;
            case EstadoPersonagem.Running:
                animador.SetBool("isRunning", true);
                break;
            case EstadoPersonagem.IdleSwimming:
                animador.SetBool("isIdleSwimming", true);
                break;
            case EstadoPersonagem.Swimming:
                animador.SetBool("isSwimming", true);
                break;
        }
    }

    private void MoverPersonagem()
    {
        if (mainCamera == null) return;

        Vector3 frente = mainCamera.transform.forward;
        Vector3 direita = mainCamera.transform.right;

        frente.y = 0f;
        direita.y = 0f;
        frente.Normalize();
        direita.Normalize();

        direcaoMovimento = frente * inputMovimento.y + direita * inputMovimento.x;

        if (direcaoMovimento.magnitude > 0.1f)
        {
            Quaternion rotacaoAlvo = Quaternion.LookRotation(direcaoMovimento);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacaoAlvo, Time.deltaTime * 10f);
        }

        rb.velocity = direcaoMovimento * velocidadeAtual;
    }

    private void MoverNaAgua()
    {
        if (inputMovimento.magnitude > 0.1f)
        {
            Vector3 movimento = new Vector3(inputMovimento.x, 0, inputMovimento.y);
            movimento = mainCamera.transform.TransformDirection(movimento);
            movimento.y = 0f;

            Vector3 desiredVelocity = movimento.normalized * velocidadeAtual;
            rb.velocity = Vector3.Lerp(rb.velocity, desiredVelocity, Time.deltaTime * 2f);

            Quaternion rotacaoAlvo = Quaternion.LookRotation(movimento.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacaoAlvo, Time.deltaTime * 5f);
        }
        else
        {
            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.deltaTime * 2f);
        }
    }

    private void AtivarMovimentoNaAgua()
    {
        usandoRigidbody = true;
        rb.isKinematic = false;
        rb.drag = 5f;
        rb.angularDrag = 2f;
    }

    private void AtivarMovimentoEmTerra()
    {
        usandoRigidbody = false;
        rb.isKinematic = true;
        rb.drag = 0f;
        rb.angularDrag = 0.05f;
    }

    public bool EstaMovendo()
    {
        return direcaoMovimento.magnitude > 0.1f;
    }
}
