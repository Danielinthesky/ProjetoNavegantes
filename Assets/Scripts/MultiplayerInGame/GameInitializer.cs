using System.Collections;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    public static GameInitializer Instance { get; private set; }

    private bool componentesCarregados = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persiste entre cenas
        }
        else
        {
            Destroy(gameObject); // Evita duplicatas
        }
    }

    public IEnumerator InicializarComponentes(GameObject jogador)
    {
        Debug.Log("Inicializando componentes para: " + jogador.name);

        // Aguarda um frame para garantir que todos os componentes sejam carregados
        yield return null;

        // Inicializa componentes críticos
        var stamina = jogador.GetComponent<StaminaPersonagemMultiplayer>();
        if (stamina == null)
        {
            Debug.LogError("StaminaPersonagemMultiplayer ausente no jogador: " + jogador.name);
        }

        var waterFloat = jogador.GetComponent<WaterFloatMultiplayer>();
        if (waterFloat == null)
        {
            Debug.LogError("WaterFloatMultiplayer ausente no jogador: " + jogador.name);
        }

        // Marque como concluído
        componentesCarregados = true;
        Debug.Log("Componentes carregados para: " + jogador.name);
    }

    public bool ComponentesEstaoCarregados()
    {
        return componentesCarregados;
    }
}
