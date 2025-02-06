using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;



[GenerateSerializationForType(typeof(string))]
public class SalaUIManager : NetworkBehaviour
{
    // Botões do lobby (representam as 3 salas)
    public Button botaoSala1;
    public Button botaoSala2;
    public Button botaoSala3;

    // Botões que representam os jogadores dentro da sala (visíveis somente para o jogador que está na sala)
    public Button botaoJogador1;
    public Button botaoJogador2;

    // NetworkVariables para o estado dos botões das salas no lobby
    public NetworkVariable<NetworkString> estadoSala1 = new NetworkVariable<NetworkString>("Sala 1");
    public NetworkVariable<NetworkString> estadoSala2 = new NetworkVariable<NetworkString>("Sala 2");
    public NetworkVariable<NetworkString> estadoSala3 = new NetworkVariable<NetworkString>("Sala 3");

    // E para os jogadores:
    public NetworkVariable<NetworkString> jogador1Sala1 = new NetworkVariable<NetworkString>("");
    public NetworkVariable<NetworkString> jogador2Sala1 = new NetworkVariable<NetworkString>("");
// ... e assim por diante.
    public NetworkVariable<NetworkString> jogador1Sala2 = new NetworkVariable<NetworkString>("");
    public NetworkVariable<NetworkString> jogador2Sala2 = new NetworkVariable<NetworkString>("");
    public NetworkVariable<NetworkString> jogador1Sala3 = new NetworkVariable<NetworkString>("");
    public NetworkVariable<NetworkString> jogador2Sala3 = new NetworkVariable<NetworkString>("");
    private readonly object salaLock = new object();

    // Variável local para indicar em qual sala o jogador está (0 = nenhuma)
    private int localRoom = 0;

    void Start()
    {
        if (IsServer) // Adicione esta verificação no início do método
        {
            estadoSala1.Value = "Sala 1";
            estadoSala2.Value = "Sala 2";
            estadoSala3.Value = "Sala 3";
            Debug.Log("Estados iniciais das salas configurados no servidor.");
        }

        // Atualiza os textos iniciais dos botões do lobby a partir dos valores dos NetworkVariables
        AtualizarTextoBotao(botaoSala1, estadoSala1.Value);
        AtualizarTextoBotao(botaoSala2, estadoSala2.Value);
        AtualizarTextoBotao(botaoSala3, estadoSala3.Value);

        // Inscreve-se para atualizar a interface sempre que o estado da sala mudar
        estadoSala1.OnValueChanged += (oldVal, newVal) =>
        {
                Debug.Log($"[Cliente] Estado da Sala 1 alterado de '{oldVal}' para '{newVal}'.");

            AtualizarTextoBotao(botaoSala1, newVal);
            botaoSala1.interactable = newVal.Value.Contains("Disponivel");

        };
        estadoSala2.OnValueChanged += (oldVal, newVal) =>
        {
                Debug.Log($"[Cliente] Estado da Sala 2 alterado de '{oldVal}' para '{newVal}'.");

            AtualizarTextoBotao(botaoSala2, newVal);
            botaoSala2.interactable = newVal.Value.Contains("Disponivel");

        };
        estadoSala3.OnValueChanged += (oldVal, newVal) =>
        {
                Debug.Log($"[Cliente] Estado da Sala 3 alterado de '{oldVal}' para '{newVal}'.");

            AtualizarTextoBotao(botaoSala3, newVal);
            botaoSala3.interactable = newVal.Value.Contains("Disponivel");

        };

        var transport = NetworkManager.Singleton.NetworkConfig.NetworkTransport as UnityTransport;
        if (transport != null)
        {
            Debug.Log("UnityTransport configurado corretamente.");
            // Aqui, você pode adicionar qualquer configuração adicional, se necessário.
        }
        else
        {
            Debug.LogError("UnityTransport não foi encontrado no NetworkManager.");
        }



        // Inscreve-se para atualizar o painel de jogadores quando os nomes forem atualizados (se o jogador local estiver na sala)
        jogador1Sala1.OnValueChanged += (oldVal, newVal) => { if (localRoom == 1) AtualizarPainelJogadores(1); };
        jogador2Sala1.OnValueChanged += (oldVal, newVal) => { if (localRoom == 1) AtualizarPainelJogadores(1); };
        jogador1Sala2.OnValueChanged += (oldVal, newVal) => { if (localRoom == 2) AtualizarPainelJogadores(2); };
        jogador2Sala2.OnValueChanged += (oldVal, newVal) => { if (localRoom == 2) AtualizarPainelJogadores(2); };
        jogador1Sala3.OnValueChanged += (oldVal, newVal) => { if (localRoom == 3) AtualizarPainelJogadores(3); };
        jogador2Sala3.OnValueChanged += (oldVal, newVal) => { if (localRoom == 3) AtualizarPainelJogadores(3); };

        // Associa os eventos de clique dos botões das salas
        botaoSala1.onClick.AddListener(() => AoClicarNoBotaoSala(1));
        botaoSala2.onClick.AddListener(() => AoClicarNoBotaoSala(2));
        botaoSala3.onClick.AddListener(() => AoClicarNoBotaoSala(3));

        // Inicialmente, o painel de jogadores (botões de Jogador 1 e Jogador 2) fica oculto
        botaoJogador1.gameObject.SetActive(false);
        botaoJogador2.gameObject.SetActive(false);
    }

    // Atualiza o texto de um botão usando TMP_Text (TextMeshProUGUI)
    private void AtualizarTextoBotao(Button botao, string texto)
    {
        TMP_Text tmp = botao.GetComponentInChildren<TMP_Text>();
        if (tmp != null)
        {
            tmp.text = texto;
        }
        else
        {
            Debug.LogWarning("Componente TMP_Text não encontrado em " + botao.name);
        }
    }

    // Chamado quando o jogador clica em um dos botões de sala
    public void AoClicarNoBotaoSala(int numeroSala)
    {
        bool salasCheias = 
            !string.IsNullOrEmpty(jogador1Sala1.Value) && !string.IsNullOrEmpty(jogador2Sala1.Value) &&
            !string.IsNullOrEmpty(jogador1Sala2.Value) && !string.IsNullOrEmpty(jogador2Sala2.Value) &&
            !string.IsNullOrEmpty(jogador1Sala3.Value) && !string.IsNullOrEmpty(jogador2Sala3.Value);

        if (salasCheias)
        {
            Debug.LogError("Todas as salas estão cheias. Nenhum novo jogador pode entrar.");
            return;
        }



        if (localRoom != 0)
        {
            Debug.Log("Você já está em uma sala.");
            return;
        }

        // Registra localmente a sala escolhida
        localRoom = numeroSala;

        // Se este for o servidor, atualiza diretamente; se for cliente, solicita ao servidor via ServerRPC
        if (IsServer)
        {
            AdicionarJogadorServidor(numeroSala, "Jogador Servidor");
        }
        else
        {
            EntrarNaSalaServerRpc(numeroSala, "Jogador Cliente");
        }
    }

    // Método executado no servidor para adicionar o jogador à sala e atualizar o estado
    public void AdicionarJogadorServidor(int numeroSala, string nomeJogador)
{   
    Debug.Log($"Tentando adicionar {nomeJogador} na sala {numeroSala}.");

    switch (numeroSala)
        {
            case 1:
                if (!string.IsNullOrEmpty(jogador1Sala1.Value) && !string.IsNullOrEmpty(jogador2Sala1.Value))
                {
                    Debug.LogError("Sala 1 cheia. Não é possível adicionar mais jogadores.");
                    return;
                }
                // Adiciona o jogador na sala 1
                if (string.IsNullOrEmpty(jogador1Sala1.Value))
                {
                    jogador1Sala1.Value = nomeJogador;
                    Debug.Log($"Jogador 1 da Sala 1 agora é {jogador1Sala1.Value}");
                }
                else if (string.IsNullOrEmpty(jogador2Sala1.Value))
                {
                    jogador2Sala1.Value = nomeJogador;
                    Debug.Log($"Jogador 2 da Sala 1 agora é {jogador2Sala1.Value}");
                }
                break;

            case 2:
                if (!string.IsNullOrEmpty(jogador1Sala2.Value) && !string.IsNullOrEmpty(jogador2Sala2.Value))
                {
                    Debug.LogError("Sala 2 cheia. Não é possível adicionar mais jogadores.");
                    return;
                }
                // Adiciona o jogador na sala 2
                if (string.IsNullOrEmpty(jogador1Sala2.Value))
                {
                    jogador1Sala2.Value = nomeJogador;
                    Debug.Log($"Jogador 1 da Sala 2 agora é {jogador1Sala2.Value}");
                }
                else if (string.IsNullOrEmpty(jogador2Sala2.Value))
                {
                    jogador2Sala2.Value = nomeJogador;
                    Debug.Log($"Jogador 2 da Sala 2 agora é {jogador2Sala2.Value}");
                }
                break;

            case 3:
                if (!string.IsNullOrEmpty(jogador1Sala3.Value) && !string.IsNullOrEmpty(jogador2Sala3.Value))
                {
                    Debug.LogError("Sala 3 cheia. Não é possível adicionar mais jogadores.");
                    return;
                }
                // Adiciona o jogador na sala 3
                if (string.IsNullOrEmpty(jogador1Sala3.Value))
                {
                    jogador1Sala3.Value = nomeJogador;
                    Debug.Log($"Jogador 1 da Sala 3 agora é {jogador1Sala3.Value}");
                }
                else if (string.IsNullOrEmpty(jogador2Sala3.Value))
                {
                    jogador2Sala3.Value = nomeJogador;
                    Debug.Log($"Jogador 2 da Sala 3 agora é {jogador2Sala3.Value}");
                }
                break;
        }

        // Log para verificar o estado da sala antes e depois da atualização
        Debug.Log($"Estado atual antes da atualização: Sala 1: {estadoSala1.Value}, Sala 2: {estadoSala2.Value}, Sala 3: {estadoSala3.Value}");

        AtualizarEstadoSalaAposEntrada(numeroSala);

        Debug.Log($"Estado atualizado após entrada: Sala 1: {estadoSala1.Value}, Sala 2: {estadoSala2.Value}, Sala 3: {estadoSala3.Value}");

        AtualizarPainelJogadores(numeroSala);
    }


    // ServerRPC para que os clientes solicitem a entrada em uma sala
    [ServerRpc(RequireOwnership = false)]
    public void EntrarNaSalaServerRpc(int numeroSala, string nomeJogador, ServerRpcParams rpcParams = default)
    {
        AdicionarJogadorServidor(numeroSala, nomeJogador);
    }

    // Atualiza o estado do botão da sala no lobby (estado global, sincronizado para todos)
    private void AtualizarEstadoSalaAposEntrada(int numeroSala)
    {
        lock (salaLock)
        {
            switch (numeroSala)
            {
                case 1:
                    if (!string.IsNullOrEmpty(jogador1Sala1.Value) && string.IsNullOrEmpty(jogador2Sala1.Value))
                    {
                        estadoSala1.Value = "Sala 1 - Disponivel, aguardando segundo jogador";
                        Debug.Log($"[Sala 1] Atualizada: {estadoSala1.Value}. Jogador 1: {jogador1Sala1.Value}, Jogador 2: {jogador2Sala1.Value}");
                    }
                    else if (!string.IsNullOrEmpty(jogador1Sala1.Value) && !string.IsNullOrEmpty(jogador2Sala1.Value))
                    {
                        estadoSala1.Value = "Sala Ocupada, Jogo prestes a iniciar";
                        Debug.Log($"[Sala 1] Atualizada: {estadoSala1.Value}. Jogador 1: {jogador1Sala1.Value}, Jogador 2: {jogador2Sala1.Value}");
                    }
                    break;

                case 2:
                    if (!string.IsNullOrEmpty(jogador1Sala2.Value) && string.IsNullOrEmpty(jogador2Sala2.Value))
                    {
                        estadoSala2.Value = "Sala 2 - Disponivel, aguardando segundo jogador";
                        Debug.Log($"[Sala 2] Atualizada: {estadoSala2.Value}. Jogador 1: {jogador1Sala2.Value}, Jogador 2: {jogador2Sala2.Value}");
                    }
                    else if (!string.IsNullOrEmpty(jogador1Sala2.Value) && !string.IsNullOrEmpty(jogador2Sala2.Value))
                    {
                        estadoSala2.Value = "Sala Ocupada, Jogo prestes a iniciar";
                        Debug.Log($"[Sala 2] Atualizada: {estadoSala2.Value}. Jogador 1: {jogador1Sala2.Value}, Jogador 2: {jogador2Sala2.Value}");
                    }
                    break;

                case 3:
                    if (!string.IsNullOrEmpty(jogador1Sala3.Value) && string.IsNullOrEmpty(jogador2Sala3.Value))
                    {
                        estadoSala3.Value = "Sala 3 - Disponivel, aguardando segundo jogador";
                        Debug.Log($"[Sala 3] Atualizada: {estadoSala3.Value}. Jogador 1: {jogador1Sala3.Value}, Jogador 2: {jogador2Sala3.Value}");
                    }
                    else if (!string.IsNullOrEmpty(jogador1Sala3.Value) && !string.IsNullOrEmpty(jogador2Sala3.Value))
                    {
                        estadoSala3.Value = "Sala Ocupada, Jogo prestes a iniciar";
                        Debug.Log($"[Sala 3] Atualizada: {estadoSala3.Value}. Jogador 1: {jogador1Sala3.Value}, Jogador 2: {jogador2Sala3.Value}");
                    }
                    break;
            }

            // Log geral para confirmar o método foi chamado
            Debug.Log($"Estado atualizado para Sala {numeroSala}.");
        }
    }


    // Atualiza o painel dos jogadores (botões que representam Jogador 1 e Jogador 2) apenas para o jogador local que está na sala
    private void AtualizarPainelJogadores(int numeroSala)
    {
        if (localRoom != numeroSala)
        {
            botaoJogador1.gameObject.SetActive(false);
            botaoJogador2.gameObject.SetActive(false);
            return;
        }

        switch (numeroSala)
        {
            case 1:
                if (!string.IsNullOrEmpty(jogador1Sala1.Value))
                {
                    botaoJogador1.gameObject.SetActive(true);
                    AtualizarTextoBotao(botaoJogador1, jogador1Sala1.Value);
                }
                else
                {
                    botaoJogador1.gameObject.SetActive(false);
                }
                if (!string.IsNullOrEmpty(jogador2Sala1.Value))
                {
                    botaoJogador2.gameObject.SetActive(true);
                    AtualizarTextoBotao(botaoJogador2, jogador2Sala1.Value);
                }
                else
                {
                    botaoJogador2.gameObject.SetActive(false);
                }
                break;
            case 2:
                if (!string.IsNullOrEmpty(jogador1Sala2.Value))
                {
                    botaoJogador1.gameObject.SetActive(true);
                    AtualizarTextoBotao(botaoJogador1, jogador1Sala2.Value);
                }
                else
                {
                    botaoJogador1.gameObject.SetActive(false);
                }
                if (!string.IsNullOrEmpty(jogador2Sala2.Value))
                {
                    botaoJogador2.gameObject.SetActive(true);
                    AtualizarTextoBotao(botaoJogador2, jogador2Sala2.Value);
                }
                else
                {
                    botaoJogador2.gameObject.SetActive(false);
                }
                break;
            case 3:
                if (!string.IsNullOrEmpty(jogador1Sala3.Value))
                {
                    botaoJogador1.gameObject.SetActive(true);
                    AtualizarTextoBotao(botaoJogador1, jogador1Sala3.Value);
                }
                else
                {
                    botaoJogador1.gameObject.SetActive(false);
                }
                if (!string.IsNullOrEmpty(jogador2Sala3.Value))
                {
                    botaoJogador2.gameObject.SetActive(true);
                    AtualizarTextoBotao(botaoJogador2, jogador2Sala3.Value);
                }
                else
                {
                    botaoJogador2.gameObject.SetActive(false);
                }
                break;
        }
    }
}
