using UnityEngine;
using TMPro;

/// <summary>
/// Script para digitar textos letra a letra usando um array (vetor) de strings,
/// sem corrotinas. Cada mensagem é exibida por completo antes de partir para a próxima.
/// </summary>
public class DigitarTextos : MonoBehaviour
{
    [TextArea] 
    public string[] mensagens;           // Vetor com as mensagens que serão digitadas
    
    public TMP_Text textoUI;             // Componente de texto (TextMeshPro) para exibir
    public float tempoEntreLetras = 0.05f; // Tempo (em segundos) entre cada letra

    private int indiceMensagemAtual = 0; // Qual mensagem do vetor está sendo exibida agora
    private float temporizador = 0f;     // Controla o tempo para soltar a próxima letra
    private int letrasReveladas = 0;     // Quantas letras da mensagem atual já foram exibidas
    private bool estaDigitando = false;  // Se estamos no meio da “digitação” de uma mensagem

    void Start()
    {
        
    }

    void Update()
    {
        if (estaDigitando)
        {
            // Acumula o tempo desde o último frame
            temporizador += Time.deltaTime;

            // Se passou do intervalo especificado (tempoEntreLetras), solta a próxima letra
            if (temporizador >= tempoEntreLetras)
            {
                temporizador = 0f;
                letrasReveladas++;

                // Garante que não ultrapasse o tamanho total da mensagem
                if (letrasReveladas <= mensagens[indiceMensagemAtual].Length)
                {
                    // Exibe apenas as letras reveladas até agora
                    textoUI.text = mensagens[indiceMensagemAtual].Substring(0, letrasReveladas);
                }

                // Se já chegamos ao fim da mensagem, paramos de digitar
                if (letrasReveladas >= mensagens[indiceMensagemAtual].Length)
                {
                    estaDigitando = false;
                }
            }
        }
    }

    /// <summary>
    /// Inicia a digitação de uma mensagem específica do vetor.
    /// </summary>
    /// <param name="indice">Índice da mensagem no array 'mensagens'.</param>
    public void IniciarDigitacao(int indice)
    {
        if (indice < mensagens.Length)
        {
            indiceMensagemAtual = indice;
            letrasReveladas = 0;    // Reinicia a contagem de letras
            textoUI.text = "";      // Limpa o texto antes de começar
            estaDigitando = true;   // Marca que estamos digitando
            temporizador = 0f;      // Zera o contador de tempo
        }
    }

    /// <summary>
    /// Exibe a próxima mensagem do array, se houver.
    /// Só chama este método quando a digitação atual estiver concluída.
    /// </summary>
    public void ExibirProximaMensagem()
    {
        if (!estaDigitando)
        {
            indiceMensagemAtual++;
            if (indiceMensagemAtual < mensagens.Length)
            {
                IniciarDigitacao(indiceMensagemAtual);
            }
            else
            {
                // Aqui você pode fazer algo quando todas as mensagens tiverem sido exibidas.
                // Por exemplo, desativar um painel ou mostrar um botão.
            }
        }
    }
}
