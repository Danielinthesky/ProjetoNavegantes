using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanManager : MonoBehaviour
{
    [Header("Configurações das Ondas")]
    public float alturaDasOndas = 0.5f;   
    public float frequenciaDasOndas = 1f; 
    public float velocidadeDasOndas = 1f; 
    [Header("Referências")]
    [SerializeField] public Transform oceano; 
    private Material materialDoOceano;        

    void Start()
    {
        ConfigurarVariaveis(); 
    }

    private void ConfigurarVariaveis()
    {
        
        materialDoOceano = oceano.GetComponent<Renderer>().sharedMaterial;

        
        AtualizarMaterial();
    }

    private void AtualizarMaterial()
    {
        // Envia os parâmetros de onda para o shader
        materialDoOceano.SetFloat("_Forca_da_Onda", frequenciaDasOndas / 100);
        materialDoOceano.SetFloat("_Velocidade_da_Onda", velocidadeDasOndas / 100);
        materialDoOceano.SetFloat("_Altura_da_Onda", alturaDasOndas / 100);
    }

    public float AlturaDaAguaNaPosicao(Vector3 posicao)
    {
        // Calcula a altura da onda em uma posição específica usando a função seno para simular o shader
        float onda = Mathf.Sin((posicao.x * frequenciaDasOndas / 100) + (posicao.z * frequenciaDasOndas / 100) + (Time.time * velocidadeDasOndas / 100));
        return oceano.position.y + onda * alturaDasOndas / 100 * oceano.localScale.x;
    }

    void OnValidate()
    {
        // Atualiza as variaveis sempre que há mudanças no inspetor, assim fica mais facil editar o oceano sem ter que buscar o material toda vez
        if (materialDoOceano == null) ConfigurarVariaveis();
        AtualizarMaterial();
    }
}
