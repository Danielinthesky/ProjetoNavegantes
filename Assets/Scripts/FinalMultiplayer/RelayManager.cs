using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using TMPro; // Necessário para usar TMP_InputField

public class RelayManager : MonoBehaviour
{
    public TMP_InputField joinCodeInputField; // Campo para TMP_InputField

    public async void StartHostWithRelay()
{
    await AuthenticationManager.Authenticate();

    var allocation = await RelayService.Instance.CreateAllocationAsync(10);
    string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
    Debug.Log($"Relay Join Code (Host): {joinCode}");

    var unityTransport = NetworkManager.Singleton.GetComponent<UnityTransport>();
    unityTransport.SetRelayServerData(
        allocation.RelayServer.IpV4,
        (ushort)allocation.RelayServer.Port,
        allocation.AllocationIdBytes,
        allocation.Key,
        allocation.ConnectionData,
        allocation.ConnectionData,
        true
    );

    NetworkManager.Singleton.StartHost();
    Debug.Log("Host iniciado e conectado ao Relay!");
}


public async void StartClientWithRelay()
{
    Debug.Log("Botão Client foi clicado!");

    string joinCode = joinCodeInputField.text.ToUpper();

    Debug.Log($"Join Code capturado: {joinCode}");

    if (string.IsNullOrEmpty(joinCode))
    {
        Debug.LogError("Join Code está vazio! Verifique se foi digitado corretamente.");
        return;
    }

    try
    {
        Debug.Log("Autenticando no Relay...");
        await AuthenticationManager.Authenticate();
        Debug.Log("Autenticação concluída!");

        Debug.Log("Tentando conectar ao Relay...");
        var joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
        Debug.Log("Join Allocation recebido com sucesso!");

        var unityTransport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        unityTransport.SetRelayServerData(
            joinAllocation.RelayServer.IpV4,
            (ushort)joinAllocation.RelayServer.Port,
            joinAllocation.AllocationIdBytes,
            joinAllocation.Key,
            joinAllocation.ConnectionData,
            joinAllocation.HostConnectionData,
            true
        );

        NetworkManager.Singleton.StartClient();
        Debug.Log("Cliente conectado ao host!");
    }
    catch (System.Exception ex)
    {
        Debug.LogError($"Erro ao conectar como cliente: {ex.Message}");
    }
}




}
