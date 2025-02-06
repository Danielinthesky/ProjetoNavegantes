using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Unity.Netcode;
[GenerateSerializationForType(typeof(string))]
public class SalaItemUI : MonoBehaviour
{
    public TMP_Text nomeSalaText; // Texto que mostra o nome da sala
    public Button botao;          // O botão para clicar

    private string nomeSala;
    private Action<string> callback;

    // Chamado após instanciar para configurar o item
    public void Configurar(string nome, Action<string> aoClicar)
    {
        nomeSala = nome;
        callback = aoClicar;

        // Mostra o nome no texto
        if (nomeSalaText != null)
            nomeSalaText.text = nomeSala;

        // Evento de clique
        if (botao != null)
        {
            botao.onClick.RemoveAllListeners();
            botao.onClick.AddListener(() => callback?.Invoke(nomeSala));
        }
    }
}
