using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
[GenerateSerializationForType(typeof(string))]
public class GameNetworkManager : MonoBehaviour
{
    private DigitarTextos digitarTextos;
    public TMP_Text connectionStatusText;
    public SalaUIManager salaUIManager; // Referência ao componente que gerencia as salas
    public float typingSpeed = 0.05f;

    void Start()
    {   
        digitarTextos = GetComponent<DigitarTextos>();
        digitarTextos.IniciarDigitacao(0);

        // Inicia o jogo após um intervalo
        Invoke("StartGame", 2f);
    }

    public void StartGame()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            Debug.LogWarning("O servidor já está em execução.");
            return;
        }

        if (NetworkManager.Singleton.StartHost())
        {
            Debug.Log("Servidor iniciado com sucesso.");
            Debug.Log("Servidor está ativo e aguardando conexões.");
        }
        else
        {
            Debug.LogError("Erro: O servidor não foi iniciado.");
        }
        
        Debug.Log("Cliente tentando conectar ao servidor...");
        if (NetworkManager.Singleton.StartClient())
        {
            Debug.Log("Cliente conectado ao servidor com sucesso.");
        }
        else
        {
            Debug.LogError("Erro: Cliente não conseguiu conectar ao servidor.");
        }
    }


    private void OnStartGameButtonClicked()
    {
        // Lógica para iniciar o jogo quando o botão é clicado
        Debug.Log("Botão Iniciar Jogo clicado.");
    }

    IEnumerator TypeText(string message)
    {
        if (connectionStatusText != null)
        {
            connectionStatusText.text = ""; // Limpa o texto antes de digitar
            foreach (char letter in message.ToCharArray())
            {
                connectionStatusText.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
        }
    }

    /// <summary>
    /// Método para criar (ou entrar na) uma sala. Na lógica atual, a criação e entrada de sala
    /// são tratadas pela mesma função (AoClicarNoBotaoSala) do SalaUIManager.
    /// </summary>
    /// <param name="numeroSala">Número da sala (1, 2 ou 3)</param>
    public void CriarSala(int numeroSala)
    {
        if (salaUIManager != null)
        {
            // Chama o método que já contém a lógica para criar/entrar na sala
            salaUIManager.AoClicarNoBotaoSala(numeroSala);
        }
        Debug.Log("Sala criada: Sala " + numeroSala);
    }

    /// <summary>
    /// Método para entrar na sala utilizando um nome de jogador personalizado.
    /// Se for servidor, chama a função diretamente; se for cliente, usa ServerRPC.
    /// </summary>
    /// <param name="numeroSala">Número da sala (1, 2 ou 3)</param>
    /// <param name="nomeJogador">Nome do jogador</param>
    public void EntrarNaSala(int numeroSala, string nomeJogador)
    {
        if (salaUIManager != null)
        {
            if (NetworkManager.Singleton.IsServer)
            {
                salaUIManager.AdicionarJogadorServidor(numeroSala, nomeJogador);
            }
            else
            {
                salaUIManager.EntrarNaSalaServerRpc(numeroSala, nomeJogador);
            }
        }
        Debug.Log($"{nomeJogador} entrou na Sala {numeroSala}");
    }
}
