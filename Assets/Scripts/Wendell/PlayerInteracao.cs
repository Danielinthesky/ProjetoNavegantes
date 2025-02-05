using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.UI; // Não esqueça de incluir para trabalhar com UI

public class PlayerInteracao : MonoBehaviour
{
    private GameObject objetoInteragivel;
    public GameObject botaoInteracao;
    
    // Componente Image do botão que exibirá o ícone
    public Image imagemInteracao;

    // Sprites dos ícones para cada tipo de interação
    public Sprite iconeEscalada;
    public Sprite iconeColeta;
    public Sprite iconeQuest;

    private GameObject gerenciadorDeMissoes;
    private bool debounce = false;
    public bool interagindo = false;
    private PlayerMovimento playerMovimento;

    void Start()
    {
        // Encontra o objeto gerenciador de missões no início
        gerenciadorDeMissoes = GameObject.Find("GerenciadorDeMissoes");
        playerMovimento = GetComponent<PlayerMovimento>();
    }

    public void HandleTriggerEnter(Collider other)
    {
        // Verifica se o objeto possui uma das tags desejadas
        if (other.CompareTag("Climb1") || other.CompareTag("Coletavel") || other.CompareTag("Quest"))
        {
            objetoInteragivel = other.gameObject;
            botaoInteracao.SetActive(true); // Exibe o botão de interação

            // Atualiza o ícone de acordo com o tipo de interação
            if (other.CompareTag("Climb1"))
            {
                if (imagemInteracao != null && iconeEscalada != null)
                    imagemInteracao.sprite = iconeEscalada;
            }
            else if (other.CompareTag("Coletavel"))
            {
                if (imagemInteracao != null && iconeColeta != null)
                    imagemInteracao.sprite = iconeColeta;
            }
            else if (other.CompareTag("Quest"))
            {
                if (imagemInteracao != null && iconeQuest != null)
                    imagemInteracao.sprite = iconeQuest;
            }
        }
    }

    public void HandleTriggerExit(Collider other)
    {
        if (other.CompareTag("Climb1") || other.CompareTag("Coletavel") || other.CompareTag("Quest"))
        {
            objetoInteragivel = null;
            botaoInteracao.SetActive(false); // Oculta o botão de interação
        }
    }

    public void RealizarInteracao()
    {
        if (interagindo) // Caso já esteja interagindo, sai da interação
        {
            SairInteracao();
            return;
        }

        if (objetoInteragivel == null) return;

        if (objetoInteragivel.CompareTag("Climb1"))
        {
            Debug.Log("Iniciando escalada...");
            GetComponent<PlayerEscalada>().IniciarEscalada(); // Chama o script de escalada
        }
        else if (objetoInteragivel.CompareTag("Coletavel"))
        {
            Debug.Log($"Coletando objeto: {objetoInteragivel.name}");
            // Adicione a lógica de coleta aqui
        }
        else if (objetoInteragivel.CompareTag("Quest"))
        {
            Debug.Log("Interagindo com NPC...");
            PrimeiraMissaoMaori missao = gerenciadorDeMissoes.GetComponent<PrimeiraMissaoMaori>();
            if (missao != null)
            {
                IniciarInteracaoNPC(missao);
            }
            else
            {
                Debug.LogWarning("O gerenciador de missões não possui o script PrimeiraMissaoMaori.");
            }
        }
        else
        {
            Debug.LogWarning("Nenhuma ação definida para esta interação.");
        }
    }

    private void IniciarInteracaoNPC(PrimeiraMissaoMaori missao)
    {
        // Desativa o movimento do jogador
        playerMovimento.enabled = false;
        interagindo = true;

        // Chama o método de interação do NPC
        missao.IniciarInteracao(objetoInteragivel);
    }

    public void SairInteracao()
    {
        // Reativa o movimento do jogador
        playerMovimento.enabled = true;
        interagindo = false;

        // Oculta o painel e reativa a câmera do jogador
        PrimeiraMissaoMaori missao = gerenciadorDeMissoes.GetComponent<PrimeiraMissaoMaori>();
        if (missao != null)
        {
            missao.SairInteracao();
        }
    }

    public void HandlePular(InputAction.CallbackContext contexto, Rigidbody rb, Animator animador, PlayerMovimento movimento, float forcaPulo)
    {
        if (contexto.performed && movimento.EstaNoChao)
        {
            rb.velocity = new Vector3(rb.velocity.x, forcaPulo, rb.velocity.z);
            animador.SetTrigger(movimento.EstaMovendo ? "PularEmMovimento" : "PularParado");
        }
    }

    private IEnumerator ResetDebounce()
    {
        yield return new WaitForSeconds(0.5f);
        debounce = false;
    }
}
