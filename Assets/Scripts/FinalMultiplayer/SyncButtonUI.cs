using Unity.Netcode;
using UnityEngine;

public class NetworkStart : MonoBehaviour
{
    public void StartHost()
    {
        Debug.Log("Tentando iniciar como Host...");
        if (NetworkManager.Singleton.StartHost())
        {
            Debug.Log("Host iniciado com sucesso!");
        }
        else
        {
            Debug.LogError("Falha ao iniciar o Host.");
        }
    }

    public void StartClient()
    {
        Debug.Log("Tentando conectar como Cliente...");
        if (NetworkManager.Singleton.StartClient())
        {
            Debug.Log("Cliente tentando se conectar...");
        }
        else
        {
            Debug.LogError("Falha ao iniciar o Cliente.");
        }
    }
}
