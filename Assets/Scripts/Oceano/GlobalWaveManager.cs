using UnityEngine;

public class GlobalWaveManager : MonoBehaviour
{
    // Referência ao material do oceano (Shader de água)
    public Material oceanMaterial;
    
    // Variáveis internas para armazenar os parâmetros do shader
    private float waveStrength;
    private float waveSpeed;
    private Vector2 waveDirection;
    private float waveHeight;
    private float timeOffset = 0f;

    void Start()
    {
        if (oceanMaterial == null)
        {
            Debug.LogError("Material de oceano não atribuído!");
            enabled = false;
            return;
        }

        // Inicializa os valores dos parâmetros com base no material
        UpdateShaderParameters();
    }

    void Update()
    {
        // Atualiza o deslocamento de tempo com a velocidade da onda
        timeOffset += Time.deltaTime * waveSpeed;

        // Atualiza os parâmetros do shader em tempo real (caso mudem)
        UpdateShaderParameters();
    }

    void UpdateShaderParameters()
    {
        // Carregar os parâmetros diretamente do material
        waveStrength = oceanMaterial.GetFloat("_wave_strength");
        waveSpeed = oceanMaterial.GetFloat("_wave_speed");
        waveDirection = oceanMaterial.GetVector("_wave_direction");
        waveHeight = oceanMaterial.GetFloat("_wave_height");
    }

    // Função para calcular a altura da onda em um ponto específico (x, z) sincronizado com o shader
    public float GetWaveHeight(float x, float z)
    {
        // Converte a direção da onda para normalizar e aplica a intensidade da onda
        float waveX = Mathf.Sin(x * waveStrength * waveDirection.x + timeOffset) * waveHeight;
        float waveZ = Mathf.Sin(z * waveStrength * waveDirection.y + timeOffset) * waveHeight;

        // Calcula a altura combinada das ondas X e Z, imitando a lógica do shader
        return waveX + waveZ;
    }
}
