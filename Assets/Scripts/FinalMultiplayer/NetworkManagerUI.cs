using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    public Button hostButton; // Botão para iniciar o Host
    public Button clientButton; // Botão para iniciar o Client

    private void Start()
    {
        hostButton.onClick.AddListener(StartHost);
        clientButton.onClick.AddListener(StartClient);
    }

    private void StartHost()
    {
        Debug.Log("Iniciando Host...");
        NetworkManager.Singleton.StartHost();
    }

    private void StartClient()
    {
        Debug.Log("Iniciando Client...");
        NetworkManager.Singleton.StartClient();
    }
}