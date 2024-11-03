using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanWaveManager : MonoBehaviour
{
    public float amplitude = 1f;       // Altura da onda
    public float wavelength = 2f;      // Comprimento da onda
    public float speed = 1f;           // Velocidade da onda
    private float offset = 0f;         // Deslocamento da onda ao longo do tempo

    private void Awake()
    {
        Debug.Log("OceanWaveManager initialized.");  // Mensagem para garantir inicialização
    }

    void Update()
    {
        // Atualiza o offset com base no tempo e na velocidade da onda
        offset += Time.deltaTime * speed;
    }

    public float GetWaveHeight(float x, float z)
    {
        // Calcula a altura da onda com base em x e z, adicionando o offset para o movimento da onda
        return amplitude * Mathf.Sin((x + z) / wavelength + offset);
    }
}
