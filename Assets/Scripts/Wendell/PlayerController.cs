using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Animator animador;
    private PlayerMovimento movimento;
    private PlayerInteracao interacao;
    private PlayerNavegacao navegacao;

    [Header("Câmeras")]
    public CinemachineFreeLook freeLookPlayer;
    public CinemachineFreeLook freeLookRaft;

    [Header("UI")]
    public GameObject canvasMovimentacao;
    public GameObject canvasNavegacao;
    public GameObject actionButton;

    [Header("Configurações de Pulo e Queda")]
    public float forcaPulo = 7.5f;

    [Header("Estados do Personagem")] // Variáveis de estado organizadas
    public bool EstaNoChao;
    public bool EstaMovendo;
    public bool EstaAndando;
    public bool EstaCorrendo;
    public bool Pulando;
    public bool EstaCaindo;
    public bool EstaNaAgua;
    public bool Nadando;

    private PlayerInput playerInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animador = GetComponent<Animator>();
        movimento = GetComponent<PlayerMovimento>();
        interacao = GetComponent<PlayerInteracao>();
        navegacao = GetComponent<PlayerNavegacao>();

        playerInput = GetComponent<PlayerInput>();
        Debug.Log("Action Map ativo: " + playerInput.currentActionMap.name);
    }

    void FixedUpdate()
    {
        AtualizarEstados();
    }

    private void AtualizarEstados()
{
    // Sincroniza estados com PlayerMovimento e Interacao
    EstaNoChao = movimento.EstaNoChao;
    EstaMovendo = movimento.EstaMovendo;
    EstaCorrendo = movimento.EstaCorrendo;

    // Atualiza EstaAndando: está andando se estiver se movendo e não estiver correndo
    EstaAndando = EstaMovendo && !EstaCorrendo;

    // Atualiza estados visuais no Animator
    animador.SetBool("EstaNoChao", EstaNoChao);
    animador.SetBool("EstaMovendo", EstaMovendo);
    animador.SetBool("EstaAndando", EstaAndando); // Adicionado
    animador.SetBool("EstaCorrendo", EstaCorrendo);
    animador.SetBool("Pulando", Pulando);
    animador.SetBool("EstaCaindo", !EstaNoChao && !Pulando);
    animador.SetBool("EstaNaAgua", EstaNaAgua);
    
}

    public void OnMover(InputAction.CallbackContext contexto)
{
    if (GetComponent<PlayerEscalada>().enabled)
    {
        GetComponent<PlayerEscalada>().OnMover(contexto.ReadValue<Vector2>());
    }
    else if (navegacao != null && navegacao.Navegando)
    {
        navegacao.HandleMover(contexto);
    }
    else
    {
        movimento.OnMover(contexto.ReadValue<Vector2>());
    }
}


    public void OnPular(InputAction.CallbackContext contexto)
    {
    Debug.Log("Evento de pulo recebido: " + contexto.phase);

    if (contexto.performed && (EstaNoChao || EstaNaAgua))
    {   
        Debug.Log("Pulo executado");    
        interacao.HandlePular(contexto, rb, animador, movimento, forcaPulo);
        Pulando = true;
    }
    }


    public void OnCorrer(InputAction.CallbackContext contexto)
    {
        movimento.OnCorrer(contexto.performed);
    }

    public void OnActionButtonPressed(InputAction.CallbackContext contexto)
    {
        if (contexto.performed)
        {
            if (interacao != null)
            {
                interacao.RealizarInteracao();
            }
            else
            {
                Debug.LogError("PlayerInteracao não está atribuído ao PlayerController!");
            }
        }
    }

    public void AtivarMovimento()
    {
        movimento.enabled = true; // Ativa o sistema de movimento
        GetComponent<PlayerEscalada>().enabled = false; // Desativa o sistema de escalada
    }

    public void AtivarEscalada()
    {
        movimento.enabled = false; // Desativa o sistema de movimento
        GetComponent<PlayerEscalada>().enabled = true; // Ativa o sistema de escalada
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Agua"))
        {
            EstaNaAgua = true;
            Nadando = true;
        }

        interacao.HandleTriggerEnter(other);
        navegacao.HandleTriggerEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Agua"))
        {
            EstaNaAgua = false;
            Nadando = false;
        }

        interacao.HandleTriggerExit(other);
        navegacao.HandleTriggerExit(other);
    }
}
