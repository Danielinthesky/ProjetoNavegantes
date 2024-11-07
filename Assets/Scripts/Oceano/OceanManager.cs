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
        // Garante que as variáveis estão configuradas no início do jogo
        if (oceano != null)
        {
            ConfigurarVariaveis();
        }
        else
        {
            Debug.LogError("A referência 'oceano' não está atribuída. Por favor, verifique no inspetor.", this);
        }
    }

    private void ConfigurarVariaveis()
    {
        // Verifica se o objeto 'oceano' possui um Renderer antes de acessar o material
        Renderer renderer = oceano?.GetComponent<Renderer>();
        if (renderer != null)
        {
            materialDoOceano = renderer.sharedMaterial;
            AtualizarMaterial();
        }
        else
        {
            Debug.LogError("O objeto 'oceano' não possui um componente Renderer.", this);
        }
    }

    private void AtualizarMaterial()
    {
        // Verifica se materialDoOceano foi configurado corretamente antes de tentar acessar
        if (materialDoOceano != null)
        {
            materialDoOceano.SetFloat("_Forca_da_Onda", frequenciaDasOndas / 100);
            materialDoOceano.SetFloat("_Velocidade_da_Onda", velocidadeDasOndas / 100);
            materialDoOceano.SetFloat("_Altura_da_Onda", alturaDasOndas / 100);
        }
        else
        {
            Debug.LogWarning("materialDoOceano não está atribuído, a atualização do material foi ignorada.", this);
        }
    }

    public float AlturaDaAguaNaPosicao(Vector3 posicao)
    {
        // Calcula a altura da onda em uma posição específica usando a função seno para simular o shader
        float onda = Mathf.Sin((posicao.x * frequenciaDasOndas / 100) + (posicao.z * frequenciaDasOndas / 100) + (Time.time * velocidadeDasOndas / 100));
        return oceano.position.y + onda * alturaDasOndas / 100 * oceano.localScale.x;
    }

    void OnValidate()
    {
        // Executa apenas se oceano e materialDoOceano estão devidamente atribuídos
        if (oceano == null) return;
        if (materialDoOceano == null) ConfigurarVariaveis();
        else AtualizarMaterial();
    }
}
