using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;



public class PrimeiraMissaoMaori : MonoBehaviour
{
    [Header("UI e Câmera")]
    public GameObject painelInteracao; // Painel de diálogo
    public TextMeshProUGUI textoNPC;   // Referência ao TextMeshPro
    public Camera cameraNPC; // Câmera fixa para interação
    public Camera cameraJogador; // Câmera principal do jogador
    public GameObject cinemachineJogador; // Objeto da Cinemachine do jogador
    private Coroutine coroutineFala;
    [Header("UI de Seleção")]
    public GameObject painelSelecao; // Painel contendo os botões de Sim/Não
    public GameObject confirmarSelecao;
    public GameObject botaoSim;
    public GameObject botaoNao;
    private int selecaoAtual = 0; // 0 = Sim, 1 = Não
    [Header("UI e Câmera")]
    public GameObject botaoPular;
    public GameObject botaoCorrer; // Painel contendo o texto da NPC
    private EstadoConversa estadoAtual = EstadoConversa.Introducao;
    private bool debounce = false;
    private enum EstadoConversa
    
{
    Introducao,
    OrigemNPCs,
    OrigemVinda,
    Proposito,
    AceiteMissao
}


    [Header("Missão")]
    public string falaAntesMissao = "Saudações jovem! Você com certeza me lembra alguém... Não consigo lembrar... Sem mais delongas, Pode nos ajudar ? Queremos falar com o Lider Maori sobre comercio de especiarias com o continente Tuaregue, mas ele não quer nos ver nem pintados de ouro, consegue convencer ele a falar com gente ? ";
    public string falaDepoisMissao = "Ótimo! A missão começou. Boa sorte!";
    public float velocidadeFalaNPC = 0.01f;
    private GameObject npcAtual;
    public bool MissaoAceita = false;
    private PlayerInteracao playerInteracao;
   
  


    private void Start()
    {
        playerInteracao = GameObject.FindWithTag("Jogador").GetComponent<PlayerInteracao>();
    }
    
    public void IniciarInteracao(GameObject npc)
    {
        npcAtual = npc;

        // Ativa a câmera do NPC e desativa a do jogador
        cameraNPC.gameObject.SetActive(true);
        cameraJogador.gameObject.SetActive(false);
        cinemachineJogador.SetActive(false);

        // Interrompe qualquer corrotina ativa e reseta o texto
        if (coroutineFala != null)
        {
            StopCoroutine(coroutineFala);
            coroutineFala = null; // Garante que a referência seja liberada
        }

        textoNPC.text = ""; // Limpa o texto
        painelInteracao.SetActive(true); // Ativa o painel de interação

        // Decide qual fala exibir com base no estado da missão
        if (!MissaoAceita)
        {
            coroutineFala = StartCoroutine(ExibirFala(falaAntesMissao));
        }
        else
        {
            coroutineFala = StartCoroutine(ExibirFala(falaDepoisMissao));
        }
    }

    
    public void SairInteracao()
    {
        estadoAtual = EstadoConversa.Introducao; // Reinicia o estado
        cameraNPC.gameObject.SetActive(false);
        cameraJogador.gameObject.SetActive(true);
        cinemachineJogador.SetActive(true);
        painelInteracao.SetActive(false);
        painelSelecao.SetActive(false);
    }



    public void AceitarMissao()
    {
        Debug.Log("Missão aceita!");
        MissaoAceita = true; // Define que a missão foi aceita

        // Fecha o painel e retorna à câmera do jogador
        painelInteracao.SetActive(false);
        cameraNPC.gameObject.SetActive(false);
        cameraJogador.gameObject.SetActive(true);
        cinemachineJogador.SetActive(true);
    }


    private void ExibirSelecao()
    {
        painelSelecao.SetActive(true); // Ativa o painel de seleção
        botaoSim.SetActive(true);
        botaoNao.SetActive(true);
        AtualizarTextoBotoes();
    }


    public void NavegarSelecao(InputAction.CallbackContext contexto)
    {
        // Verifica se o jogador está interagindo antes de permitir a navegação
        if (playerInteracao == null || !playerInteracao.interagindo) return;

        Vector2 direcao = contexto.ReadValue<Vector2>();

        if (direcao.x < -0.5f) // Para a esquerda
        {
            selecaoAtual = 0; // Sim
            AtualizarSelecao();
        }
        else if (direcao.x > 0.5f) // Para a direita
        {
            selecaoAtual = 1; // Não
            AtualizarSelecao();
        }
    }

    private void AtualizarSelecao()
    {
        // Altera o destaque visual entre os botões
        botaoSim.GetComponent<Image>().color = selecaoAtual == 0 ? Color.yellow : Color.white;
        botaoNao.GetComponent<Image>().color = selecaoAtual == 1 ? Color.yellow : Color.white;
    }

    public void ConfirmarSelecao()
    {
        if (debounce) return; // Evita seleção dupla automática

        debounce = true; // Agora o debounce realmente funciona
        StartCoroutine(ResetDebounce());

        if (selecaoAtual == 0) // Opção A -> Avança no fluxo correto
        {
            AvancarConversa();
        }
        else // Opção B -> Sai da interação e reseta
        {
            SairInteracao();
        }
    }


    private void AvancarConversa()
    {
        switch (estadoAtual)
        {
            case EstadoConversa.Introducao:
                estadoAtual = EstadoConversa.OrigemNPCs;
                break;
            case EstadoConversa.OrigemNPCs:
                estadoAtual = EstadoConversa.OrigemVinda;
                break;
            case EstadoConversa.OrigemVinda:
                estadoAtual = EstadoConversa.Proposito;
                break;
            case EstadoConversa.Proposito:
                AceitarMissao();
                return;
        }

        // Agora chamamos a digitação ANTES de exibir os botões
        StartCoroutine(ExibirFala(textos[estadoAtual]));
    }



    private void AtualizarTextoBotoes()
    {
        // Obtém os componentes TextMeshProUGUI dos botões
        TextMeshProUGUI textoBotaoA = botaoSim.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI textoBotaoB = botaoNao.GetComponentInChildren<TextMeshProUGUI>();

        // Define os textos das opções com base no estado atual
        textoBotaoA.text = opcoes[estadoAtual].Item1;
        textoBotaoB.text = opcoes[estadoAtual].Item2;

        // Ajusta automaticamente o tamanho da fonte para caber no botão
        AjustarTamanhoFonte(textoBotaoA);
        AjustarTamanhoFonte(textoBotaoB);
    }


    private void AjustarTamanhoFonte(TextMeshProUGUI textoBotao)
    {
        textoBotao.enableAutoSizing = true; // Ativa o ajuste automático de tamanho
        textoBotao.fontSizeMin = 14f; // Define um tamanho mínimo para evitar letras muito pequenas
        textoBotao.fontSizeMax = 30f; // Define um tamanho máximo para evitar textos muito grandes
        textoBotao.enableWordWrapping = true; // Permite que o texto quebre linha automaticamente
    }


    private IEnumerator ExibirFala(string texto)
    {
        painelSelecao.SetActive(false); // Garante que os botões estão ocultos
        botaoSim.SetActive(false);
        botaoNao.SetActive(false);
        textoNPC.text = ""; // Limpa o texto antes de iniciar

        foreach (char letra in texto)
        {
            textoNPC.text += letra;
            yield return new WaitForSeconds(velocidadeFalaNPC); // Digitação letra por letra
        }

        yield return new WaitForSeconds(0.3f); // Pequeno delay antes das opções aparecerem

        // Exibe as opções apenas após o texto ser digitado completamente
        ExibirSelecao();
    }




    private Dictionary<EstadoConversa, string> textos = new Dictionary<EstadoConversa, string>
    {
        { EstadoConversa.Introducao, "Olá, somos comerciantes buscando ajuda para entrar na aldeia!" },
        { EstadoConversa.OrigemNPCs, "Somos da tribo Tuaregue, famosos por nosso comércio de especiarias." },
        { EstadoConversa.OrigemVinda, "Viemos de uma terra distante ao norte, cruzando desertos e mares." },
        { EstadoConversa.Proposito, "A aldeia nos baniu por mal-entendidos do passado." },
        { EstadoConversa.AceiteMissao, "Precisamos de sua ajuda para entrar na aldeia." }
    };

    private Dictionary<EstadoConversa, (string, string)> opcoes = new Dictionary<EstadoConversa, (string, string)>
    {
        { EstadoConversa.Introducao, ("Quem são vocês?", "Não quero ajudar") },
        { EstadoConversa.OrigemNPCs, ("De onde vocês vieram?", "Entendo, não posso ajudar") },
        { EstadoConversa.OrigemVinda, ("Por que não podem entrar na aldeia?", "Melhor eu ir embora") },
        { EstadoConversa.Proposito, ("Posso ajudar!", "Não ajudarei") }
    };

    private IEnumerator ResetDebounce()
    {
        yield return new WaitForSeconds(0.5f);
        debounce = false;
    }

}
