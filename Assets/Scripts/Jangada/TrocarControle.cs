using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Cinemachine;
using Photon.Pun;

public class TrocarControle : MonoBehaviourPun
{
    public GameObject jogador;
    public GameObject jangada;
    public CinemachineFreeLook cameraJogador;
    public CinemachineFreeLook cameraJangada;
    public Button botaoTrocaControle; // Botão na UI para alternar controle

    private bool controlandoJangada = false;

    private void Start()
    {
        //if (!photonView.IsMine) return;

        DefinirModoDeControle(false);
        botaoTrocaControle.gameObject.SetActive(false); // Esconde o botão no início
        botaoTrocaControle.onClick.AddListener(AoAlternarControle); // Adiciona evento ao botão
    }

    private void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Mastro"))
    {
        Debug.Log("Colidiu com o mastro!"); // Mensagem para verificar a colisão
        botaoTrocaControle.gameObject.SetActive(true); // Exibe o botão ao colidir com o mastro
    }
}


    

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Mastro"))
        {
            botaoTrocaControle.gameObject.SetActive(false); // Esconde o botão ao sair da área do mastro
        }
    }

    private void AoAlternarControle()
    {
        controlandoJangada = !controlandoJangada;
        DefinirModoDeControle(controlandoJangada);
    }

    private void DefinirModoDeControle(bool controlarJangada)
{
    if (jogador == null || jangada == null || cameraJogador == null || cameraJangada == null)
    {
        Debug.LogError("Certifique-se de que todas as referências estão atribuídas no Inspector.");
        return;
    }

    if (controlarJangada)
    {
        jangada.GetComponent<RaftController>().enabled = true;
        jogador.GetComponent<PlayerControllerMultiplayer1>().enabled = false;

        cameraJangada.Priority = 10;
        cameraJogador.Priority = 0;
    }
    else
    {
        jogador.GetComponent<PlayerControllerMultiplayer1>().enabled = true;
        jangada.GetComponent<RaftController>().enabled = false;

        cameraJogador.Priority = 10;
        cameraJangada.Priority = 0;
    }
}

}
