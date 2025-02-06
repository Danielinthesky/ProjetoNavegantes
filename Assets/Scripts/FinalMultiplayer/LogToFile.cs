using UnityEngine;
using TMPro;

public class DebugLogger : MonoBehaviour
{
    public TextMeshProUGUI debugText; // Referência ao TMP na UI

    private void Awake()
    {
        Application.logMessageReceived += HandleLog;
    }

    private void OnDestroy()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        debugText.text += logString + "\n"; // Adiciona novas mensagens ao TMP
    }
}
