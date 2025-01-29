using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

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
    public GameObject botaoSim;
    public GameObject botaoNao;
    private int selecaoAtual = 0; // 0 = Sim, 1 = Não

    [Header("Missão")]
    public string falaNPC = "Olá! Tenho uma missão para você. Aceite para começar sua jornada!";
    public float velocidadeFalaNPC = 0.01f;
    private GameObject npcAtual;
    public bool MissaoAceita = false;

    public void IniciarInteracao(GameObject npc)
{
    if (!MissaoAceita)
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

        // Ativa o painel de interação e inicia o texto
        painelInteracao.SetActive(true);
        coroutineFala = StartCoroutine(ExibirFala(falaNPC));
    }
    else
    {
        Debug.Log("Missão já foi aceita, nenhuma nova interação necessária.");
    }
}

    
   public void SairInteracao()
    {
        cameraNPC.gameObject.SetActive(false);
        cameraJogador.gameObject.SetActive(true);
        cinemachineJogador.SetActive(true);

        painelInteracao.SetActive(false);
        painelSelecao.SetActive(false); // Garante que o painel de seleção é desativado
    }


    public void AceitarMissao()
    {
        Debug.Log("Missão aceita!");

        // Fecha o painel e retorna a câmera do jogador
        painelInteracao.SetActive(false);
        cameraNPC.gameObject.SetActive(false);
        cameraJogador.gameObject.SetActive(true);
        cinemachineJogador.SetActive(true);

        // Outras lógicas da missão podem ser adicionadas aqui
    }

    private void ExibirSelecao()
    {
        painelSelecao.SetActive(true); // Ativa o painel de seleção
        AtualizarSelecao(); // Atualiza o destaque inicial do botão
    }

    public void NavegarSelecao(Vector2 entrada)
    {
        if (Mathf.Abs(entrada.x) > 0.1f) // Apenas considera o movimento horizontal
        {
            selecaoAtual = entrada.x > 0 ? 1 : 0; // 0 = Sim, 1 = Não
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
        if (selecaoAtual == 0)
        {
            // Jogador escolheu "Sim"
            Debug.Log("Missão aceita!");
            MissaoAceita = true;
        }
        else
        {
            // Jogador escolheu "Não"
            Debug.Log("Missão recusada!");
        }

        // Fecha o painel de seleção
        painelSelecao.SetActive(false);

        // Retorna à câmera do jogador
        SairInteracao();
    }


    private IEnumerator ExibirFala(string texto)
    {
        textoNPC.text = ""; // Limpa o texto antes de iniciar
        foreach (char letra in texto)
        {
            textoNPC.text += letra;
            yield return new WaitForSeconds(velocidadeFalaNPC); // Intervalo entre cada letra
        }

        ExibirSelecao();
    }
}
