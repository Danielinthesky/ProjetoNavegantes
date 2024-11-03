using UnityEngine;

public class ControladorDaAgua : MonoBehaviour
{
    public static ControladorDaAgua instancia;

    [SerializeField] private Material materialAgua;

    private float alturaOnda;
    private float velocidadeOnda;
    private float forcaOnda;
    private float larguraEspuma;
    private float suavidadeReflexo;
    private float alturaSuperficie; // Variável para a altura da superfície da água

    private Vector2 direcaoOnda;

    private void Awake()
    {
        if (instancia != null)
        {
            Destroy(this);
        }
        instancia = this;

        if (materialAgua != null)
        {
            // Inicializa parâmetros do shader, com verificações para segurança
            if (materialAgua.HasProperty("_Altura_da_Onda")) alturaOnda = materialAgua.GetFloat("_Altura_da_Onda");
            if (materialAgua.HasProperty("_Velocidade_da_Onda")) velocidadeOnda = materialAgua.GetFloat("_Velocidade_da_Onda");
            if (materialAgua.HasProperty("_Forca_da_Onda")) forcaOnda = materialAgua.GetFloat("_Forca_da_Onda");
            if (materialAgua.HasProperty("_Largura_da_Espuma")) larguraEspuma = materialAgua.GetFloat("_Largura_da_Espuma");
            if (materialAgua.HasProperty("_Suavidade_do_Reflexo")) suavidadeReflexo = materialAgua.GetFloat("_Suavidade_do_Reflexo");
            if (materialAgua.HasProperty("_Altura_da_Superficie")) alturaSuperficie = materialAgua.GetFloat("_Altura_da_Superficie");

            if (materialAgua.HasProperty("_Direcao_do_Mar"))
                direcaoOnda = materialAgua.GetVector("_Direcao_do_Mar");
            else
                Debug.LogError("A propriedade '_Direcao_do_Mar' não foi encontrada ou está com um tipo incorreto.");
        }
        else
        {
            Debug.LogError("Material de água não atribuído no ControladorDaAgua.");
        }
    }

    public float getHeightAtPosition(Vector3 position)
    {
        if (materialAgua == null) return 0f;
        // Retorna a altura da superfície com base na variável de altura da espuma
        return alturaSuperficie;
    }

    public Vector2 GetWaveDirection()
    {
        return direcaoOnda;
    }

    public float GetWaveStrength()
    {
        return forcaOnda;
    }

    public float GetFoamWidth()
    {
        return larguraEspuma;
    }

    public float GetReflectionSmoothness()
    {
        return suavidadeReflexo;
    }
}
