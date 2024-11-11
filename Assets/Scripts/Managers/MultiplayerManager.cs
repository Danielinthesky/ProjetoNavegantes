using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class MultiplayerManager : MonoBehaviourPunCallbacks
{
    public TMP_Text textoStatus; // Para mostrar o status da conexão e outras mensagens
    public TMP_InputField campoNomeSala; // Para o jogador digitar o nome da nova sala
    public GameObject botaoCriarSala; // Botão para criar sala
    public Transform listaSalasContainer; // Container onde a lista de salas será exibida
    public GameObject prefabItemSala; // Prefab para cada item da lista de salas
    public GameObject personagem1; // Referência ao personagem 1 na cena
    public GameObject personagem2; // Referência ao personagem 2 na cena
    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true; // Ativa sincronização automática de cena
        textoStatus.text = "Conectando ao servidor...";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        textoStatus.text = "Conexão com o servidor estabelecida!";
        PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        textoStatus.text = "Desconectado: " + cause.ToString();
        Debug.LogError("Desconectado: " + cause);
    }

    public override void OnRoomListUpdate(List<RoomInfo> listaSalas)
    {
        foreach (Transform child in listaSalasContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (RoomInfo infoSala in listaSalas)
        {
            if (infoSala.RemovedFromList) continue;

            GameObject itemSala = Instantiate(prefabItemSala, listaSalasContainer);
            itemSala.SetActive(true);
            itemSala.GetComponentInChildren<TMP_Text>().text = infoSala.Name;
            itemSala.GetComponentInChildren<UnityEngine.UI.Button>().onClick.AddListener(() => EntrarNaSala(infoSala.Name));
        }

        textoStatus.text = listaSalas.Count > 0 ? "Salas disponíveis:" : "Nenhuma sala disponível. Crie uma nova!";
    }

    public void FocarNoCampoNomeSala()
    {
        campoNomeSala.Select();
        campoNomeSala.ActivateInputField();
    }

    public void AoClicarCriarSala()
    {
        string nomeSala = campoNomeSala.text;
        if (string.IsNullOrEmpty(nomeSala))
        {
            textoStatus.text = "Nome da sala não pode estar vazio!";
            return;
        }

        RoomOptions opcoesSala = new RoomOptions { MaxPlayers = 2 };
        PhotonNetwork.CreateRoom(nomeSala, opcoesSala);
        textoStatus.text = "Criando sala: " + nomeSala;
    }

    public override void OnJoinedRoom()
    {
          // Chamado quando o jogador entra na sala; garante que a propriedade dos personagens está correta
        if (PhotonNetwork.IsMasterClient)
        {
            // O Master Client (primeiro jogador) mantém a posse do personagem 1
            personagem1.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
        }
        else
        {
            // O segundo jogador recebe a posse do personagem 2
            personagem2.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
        }
        textoStatus.text = "Entrou na sala: " + PhotonNetwork.CurrentRoom.Name + ". Aguardando outro jogador...";
        AtualizarListaSalasLocal(PhotonNetwork.CurrentRoom.Name);

     
    }

    public override void OnCreatedRoom()
    {
        textoStatus.text = "Sala criada: " + PhotonNetwork.CurrentRoom.Name + ". Aguardando outro jogador...";
        Debug.Log("Sala criada com sucesso!");
    }

    private void EntrarNaSala(string nomeSala)
    {
        PhotonNetwork.JoinRoom(nomeSala);
        textoStatus.text = "Tentando entrar na sala: " + nomeSala;
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        textoStatus.text = "Falha ao entrar na sala: " + message;
        Debug.LogError("Erro ao tentar entrar na sala: " + message);
    }

    public void CriarSalaAleatoria()
    {
        string nomeSalaAleatoria = "Sala_" + Random.Range(0, 10000);
        RoomOptions opcoesSala = new RoomOptions { MaxPlayers = 2 };
        PhotonNetwork.CreateRoom(nomeSalaAleatoria, opcoesSala);
        textoStatus.text = "Criando sala aleatória: " + nomeSalaAleatoria;
        AtualizarListaSalasLocal(nomeSalaAleatoria);
    }

    private void AtualizarListaSalasLocal(string nomeSala)
    {
        foreach (Transform child in listaSalasContainer)
        {
            Destroy(child.gameObject);
        }

        GameObject itemSala = Instantiate(prefabItemSala, listaSalasContainer);
        itemSala.SetActive(true);
        itemSala.GetComponentInChildren<TMP_Text>().text = nomeSala;
        itemSala.GetComponentInChildren<UnityEngine.UI.Button>().onClick.AddListener(() => EntrarNaSala(nomeSala));
        textoStatus.text = "Sala disponível: " + nomeSala;
    }

    

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // Verifica o número atual de jogadores na sala para atribuir o personagem correto
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            // O primeiro jogador controla o personagem 1
            personagem1.GetComponent<PhotonView>().TransferOwnership(newPlayer);
        }
        else if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            // O segundo jogador controla o personagem 2
            personagem2.GetComponent<PhotonView>().TransferOwnership(newPlayer);
        }

        textoStatus.text = $"{newPlayer.NickName} entrou na sala. Total de jogadores: {PhotonNetwork.CurrentRoom.PlayerCount}/2";

        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            textoStatus.text = "O jogador 2 está pronto!";
            
            if (PhotonNetwork.IsMasterClient)
            {
                textoStatus.text += " Iniciando o jogo...";
                Invoke("ComecarJogo", 2f); // Pequeno atraso para o feedback visual antes de iniciar
            }
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        textoStatus.text = $"{otherPlayer.NickName} saiu da sala. Aguardando outro jogador...";
    }

    public void ComecarJogo()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("MultiplayerTeste");
        }
    }
}
