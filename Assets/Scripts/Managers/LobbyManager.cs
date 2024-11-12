using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public TMP_Text textoStatus;
    public TMP_InputField campoNomeSala;
    public GameObject botaoCriarSala;
    public Transform listaSalasContainer;
    public GameObject prefabItemSala;

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
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

    private void EntrarNaSala(string nomeSala)
    {
        PhotonNetwork.JoinRoom(nomeSala);
        textoStatus.text = "Tentando entrar na sala: " + nomeSala;
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        textoStatus.text = "Falha ao entrar na sala: " + message;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        textoStatus.text = $"{newPlayer.NickName} entrou na sala. Total de jogadores: {PhotonNetwork.CurrentRoom.PlayerCount}/2";

        if (PhotonNetwork.CurrentRoom.PlayerCount == 2 && PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("MultiplayerTeste");
        }
    }

    // Método para o Dev Button criar uma sala aleatória
    public void CriarSalaAleatoria()
    {
        string nomeSalaAleatoria = "Sala_" + Random.Range(0, 10000);
        RoomOptions opcoesSala = new RoomOptions { MaxPlayers = 2 };
        PhotonNetwork.CreateRoom(nomeSalaAleatoria, opcoesSala);
        textoStatus.text = "Criando sala aleatória: " + nomeSalaAleatoria;
        AtualizarListaSalasLocal(nomeSalaAleatoria);
    }

    // Método auxiliar para exibir a sala criada na lista local
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
}
