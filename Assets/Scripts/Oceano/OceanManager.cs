using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanManager : MonoBehaviour
{
    [Header("Wave Settings")]
    public float wavesHeight = 0.5f;   // Altura das ondas (equivalente a _Altura_da_Onda no shader)
    public float wavesFrequency = 1f;  // Força da onda (equivalente a _Forca_da_Onda no shader)
    public float wavesSpeed = 1f;      // Velocidade da onda (equivalente a _Velocidade_da_Onda no shader)

    [Header("References")]
    [SerializeField]public Transform ocean;            // Referência ao objeto do oceano
    private Material oceanMat;         // Material do shader de água

    void Start()
    {
        SetVariables(); // Inicializa as variáveis no material
    }

    private void SetVariables()
    {
        // Obtém o material do oceano
        oceanMat = ocean.GetComponent<Renderer>().sharedMaterial;

        // Configura as variáveis iniciais no shader
        UpdateMaterial();
    }

    private void UpdateMaterial()
    {
        // Envia os parâmetros de onda para o shader
        oceanMat.SetFloat("_Forca_da_Onda", wavesFrequency/100);
        oceanMat.SetFloat("_Velocidade_da_Onda", wavesSpeed/100);
        oceanMat.SetFloat("_Altura_da_Onda", wavesHeight/100);
    }

    public float WaterHeightAtPosition(Vector3 position)
    {
        // Calcula a altura da onda em uma posição específica usando função seno para simular o shader
        float wave = Mathf.Sin((position.x * wavesFrequency/100) + (position.z * wavesFrequency/100) + (Time.time * wavesSpeed/100));
        return ocean.position.y + wave * wavesHeight/100 * ocean.localScale.x;
    }

    void OnValidate()
    {
        // Atualiza as variáveis sempre que há mudanças no inspetor
        if (oceanMat == null) SetVariables();
        UpdateMaterial();
    }
}
