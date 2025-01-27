using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using Cinemachine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Animator animador;
    private PlayerMovimento movimento;
    private PlayerEscalada escalada;
    private GameObject jangada; // Referência à jangada
    public CinemachineFreeLook freeLookPlayer;
    public CinemachineFreeLook freeLookRaft;
    private PlayerInput playerInput;


    [Header("Configurações de Pulo e Queda")]
    public float forcaPulo = 7.5f;
    public float alturaMinimaQueda = 2f; // Altura mínima para ativar a animação de queda significativa

    [Header("Configurações de Água")]
    public static bool EstaNaAgua = false; // Indica se o personagem está na água
    

    [Header("UI e Escalada")]
    
    public GameObject moveToggleButton; // Botão de alternância para a jangada
    private bool podeEscalar = false; // Indica se o personagem pode escalar
    private bool estaEscalando = false; // Indica se o personagem está no modo de escalada
    private GameObject objetoEscalavel; // Referência ao objeto escalável
    public float velocidadeEscalada = 2f; // Velocidade de movimento vertical ao escalar
    private RaftController raftController; // Referência ao script de controle da jangada
    [Header("Configurações de Estado")]
    private bool navegando = false; // Indica se o player está navegando
    public bool EstaNoChao; // Indica se o personagem está no chão
    public bool EstaMovendo; // Indica se o personagem está se movendo
    public bool EstaAndando;
    public bool EstaCorrendo;
    public bool Pulando;
    private float alturaInicialPulo;
    private bool estaCorrendo = false; // Indica se o botão de corrida está pressionado

    private Vector2 entradaMovimento; // Entrada do stick ou teclado
    private TipoAcao contextoAtual = TipoAcao.Nenhum;
    private bool debounce = false;
    public GameObject canvasMovimentacao; // Canvas com botões de Correr, Pular e Ação
    public GameObject canvasNavegacao;   // Canvas com botões de Navegar e Sair da navegação
    public GameObject actionButton;

    

void OnEnable()
{
    playerInput.actions.Enable();
    Debug.Log("Input Actions habilitados.");
}

void OnDisable()
{
    playerInput.actions.Disable();
    Debug.Log("Input Actions desabilitados.");
}


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animador = GetComponent<Animator>();

        movimento = GetComponent<PlayerMovimento>();
        escalada = GetComponent<PlayerEscalada>();

       
       
        playerInput = GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput não encontrado no GameObject!");
        }

        var navegarActionMap = playerInput.actions.FindActionMap("Jangada");
        if (navegarActionMap == null)
        {
            Debug.LogError("Action Map 'Navegar' não encontrado no InputActionAsset!");
            return;
        }
    }

public enum TipoAcao
{
    Nenhum,
    Escalada,
    Coleta,
    Interacao,
    Navegar
}


    void FixedUpdate()
{
    if (estaEscalando)
    {
        // Verifica se a escalada deve ser encerrada automaticamente
        if (!escalada.EstaEscalando())
        {
            estaEscalando = false;
            movimento.enabled = true;
        }
    }
    else
    {
        // Detecta se está no chão verificando o ângulo da superfície
        EstaNoChao = movimento.EstaNoChao && EhSuperficiePlana(movimento.NormalSuperficie);

        if (EstaNoChao)
        {
            Pulando = false;
        }

        EstaMovendo = movimento.EstaMovendo;
        EstaCorrendo = movimento.EstaCorrendo;

        // Atualiza as animações normais
        animador.SetBool("EstaNoChao", EstaNoChao);
        animador.SetBool("EstaAndando", EstaMovendo && !EstaCorrendo);
        animador.SetBool("EstaCorrendo", EstaCorrendo);
        animador.SetBool("EstaMovendo", EstaMovendo);
    }
}


public void OnActionButtonPressed()
{
    Debug.Log("O botão está funcionando!");
}



private void ColetarObjeto()
{
    if (objetoEscalavel != null) // Certifique-se de que há um objeto para coletar
    {
        Debug.Log($"Coletando objeto: {objetoEscalavel.name}");

        // Ativa a animação de coleta
        animador.SetTrigger("Coletar");

        // Desativa o movimento do personagem
        movimento.enabled = false;

        // Inicia uma corrotina para aguardar o fim da animação
        StartCoroutine(AguardarFimAnimacaoColeta());
    }
    else
    {
        Debug.Log("Nenhum objeto para coletar.");
    }
}


private void InteragirComObjeto()
{
    Debug.Log("Interagindo com objeto...");
    // Lógica de interação aqui
}


public void OnMover(InputAction.CallbackContext contexto)
{
    Vector2 movimentoInput = contexto.ReadValue<Vector2>();

    if (navegando) // Controle da jangada
    {
        if (raftController != null)
        {
            raftController.Mover(movimentoInput); // Passa o input para a jangada
            Debug.Log($"Controlando a jangada com input: {movimentoInput}");
        }
        else
        {
            Debug.LogError("raftController não atribuído!");
        }
        return; // Sai da função após controlar a jangada
    }

    // Controle do personagem
    if (movimento != null)
    {
        movimento.OnMover(movimentoInput); // Passa o input para o personagem
        Debug.Log($"Controlando o personagem com input: {movimentoInput}");
    }
}





private bool EhSuperficiePlana(Vector3 normal)
{
    // Ângulo entre a normal da superfície e Vector3.up
    float angulo = Vector3.Angle(normal, Vector3.up);

    // Considera como plana se o ângulo for menor que 45 graus (ajustável)
    return angulo < 45f;
}

    public void OnPular(InputAction.CallbackContext contexto)
{ Debug.Log("Botão de pulo pressionado!");
    if (contexto.performed && EstaNoChao || EstaNaAgua)
    {
        alturaInicialPulo = transform.position.y;

        // Aplica a força de pulo
        rb.velocity = new Vector3(rb.velocity.x, forcaPulo, rb.velocity.z);

        // Decide qual animação de pulo reproduzir
        if (EstaMovendo)
        {
            animador.SetTrigger("PularEmMovimento"); // Ativa a animação de pulo em movimento
        }
        else
        {
            animador.SetTrigger("PularParado"); // Ativa a animação de pulo parado
        }

        animador.SetBool("EstaCaindo", false); // Ainda não está caindo logo após o pulo
        Pulando = true;
    }
}


 public void TestarBotao()
    {
        Debug.Log("Botão clicado!");
    }

    public void OnCorrer(InputAction.CallbackContext contexto)
{
    bool correndo = contexto.performed;
    movimento.OnCorrer(correndo); // Passa o estado para o PlayerMovimento
    estaCorrendo = correndo;
}



    public void OnClimb()
{
    if (podeEscalar)
    {
        if (!escalada.EstaEscalando())
        {
            escalada.IniciarEscalada();
            estaEscalando = true;
            movimento.enabled = false;
        }
        else
        {
            escalada.FinalizarEscalada();
            estaEscalando = false;
            movimento.enabled = true;
        }
    }
}



    private void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Navegar"))
{
    Debug.Log("Entrou no trigger 'Navegar'.");
    raftController = other.GetComponentInParent<RaftController>();
    if (raftController == null)
    {
        Debug.LogError("RaftController não encontrado no objeto pai!");
    }
    else
    {
        Debug.Log("RaftController atribuído com sucesso.");
        contextoAtual = TipoAcao.Navegar;
    }
}

    if (other.CompareTag("Agua"))
    {
        EstaNaAgua = true;
        animador.SetBool("EstaNaAgua", true);
    }

    if (other.CompareTag("Climb1"))
    {
        // Verifica se o personagem está nas laterais (não no topo)
        float alturaPersonagem = transform.position.y;
        float alturaObjeto = other.bounds.center.y;
        float alturaObjetoTopo = alturaObjeto + (other.bounds.extents.y * 0.9f); // Considera uma margem no topo
        float alturaObjetoBase = alturaObjeto - (other.bounds.extents.y * 0.9f); // Considera uma margem na base

        if (alturaPersonagem > alturaObjetoBase && alturaPersonagem < alturaObjetoTopo)
        {
            contextoAtual = TipoAcao.Escalada;
            podeEscalar = true;
            objetoEscalavel = other.gameObject;

           
        }

         
    }

    if (other.CompareTag("Coletavel"))
    {
        contextoAtual = TipoAcao.Coleta;
        objetoEscalavel = other.gameObject;

        
    }

    actionButton.SetActive(true);
}


private void OnTriggerExit(Collider other)
{
    if (other.CompareTag("Agua"))
    {
        EstaNaAgua = false;
        animador.SetBool("EstaNaAgua", false);
    }

    if (other.CompareTag("Climb1") || other.CompareTag("Coletavel"))
    {
        contextoAtual = TipoAcao.Nenhum;

    }
    if (other.CompareTag("Navegar"))
        {
            contextoAtual = TipoAcao.Nenhum;
               
        }

        actionButton.SetActive(false);
}

private IEnumerator AguardarFimAnimacaoColeta()
{
    // Obtém o tempo de duração da animação "Coletar"
    float duracaoAnimacao = animador.GetCurrentAnimatorStateInfo(0).length;

    // Aguarda até a animação terminar
    yield return new WaitForSeconds(duracaoAnimacao);

    // Reativa o movimento do personagem
    movimento.enabled = true;

    // Destrói ou desativa o objeto coletável
    Destroy(objetoEscalavel);

    // Reseta o contexto após a coleta
    contextoAtual = TipoAcao.Nenhum;
  

    Debug.Log("Coleta concluída.");
}

private void IniciarNavegacao()
{
    Debug.Log("IniciarNavegacao chamado.");
    if (raftController == null)
    {
        Debug.LogError("RaftController é null! Não é possível iniciar navegação.");
        return;
    }

    navegando = true;

    // Desativa o movimento do personagem
    movimento.enabled = false;

    // Ativa o controle da jangada
    raftController.enabled = true;

    // Troca Action Map
    playerInput.SwitchCurrentActionMap("Jangada");

    // Alterna câmeras
    AtivarCamera(freeLookRaft, freeLookPlayer);

    // Alterna UI
    canvasMovimentacao.SetActive(false);
    canvasNavegacao.SetActive(true);

    Debug.Log("Navegação iniciada com sucesso!");
}




private IEnumerator TrocarActionMapAposFrame(string novoActionMap)
{
    yield return new WaitForEndOfFrame(); // Espera o frame atual terminar
    InputSystem.ResetHaptics(); // Limpa possíveis eventos presos no buffer

    playerInput.SwitchCurrentActionMap(novoActionMap);
    Debug.Log($"Action Map trocado para: {playerInput.currentActionMap?.name}");
}



private void FinalizarNavegacao()
{
    navegando = false;

    // Reativa o controle do personagem
    movimento.enabled = true;

    // Desativa o controle da jangada
    raftController.enabled = false;

    // Troca Action Map (opcional)
    playerInput.SwitchCurrentActionMap("Wendell");

    // Alterna câmeras
    AtivarCamera(freeLookPlayer, freeLookRaft);

    // Alterna UI
    canvasMovimentacao.SetActive(true);
    canvasNavegacao.SetActive(false);

    Debug.Log("Navegação finalizada com sucesso!");
}





     public void OnMoveTogglePressed()
    {
        if (raftController != null)
        {
            raftController.ToggleMovement();
        }
    }

   

private IEnumerator ResetDebounce()
{
    yield return new WaitForSeconds(0.5f); // Tempo para evitar cliques duplos
    debounce = false;
}

private void AtivarCamera(CinemachineFreeLook ativar, CinemachineFreeLook desativar)
    {
        ativar.Priority = 10; // Prioridade maior para ativar
        desativar.Priority = 0; // Prioridade menor para desativar
    }
}
