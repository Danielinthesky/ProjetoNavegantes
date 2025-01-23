using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class OceanMeshGenerator : MonoBehaviour
{
    public int resolucao = 50; 
    public float tamanho = 500f; 
    public Material materialDoOceano; 

    public Transform jogador; // Referência ao Transform do jogador

    private Mesh malhaOceano;
    private Vector3[] vertices;
    private int[] triangulos;
    private Vector2[] uvs; 

    void Start()
    {
        // Gera a malha do oceano
        GerarMalhaDoOceano();

        // Configura o material do oceano
        if (materialDoOceano != null)
        {
            GetComponent<MeshRenderer>().material = materialDoOceano;
        }
        else
        {
            Debug.LogError("Material de oceano não atribuído!");
        }

        // Procura automaticamente o jogador pela tag "Jogador" se não estiver atribuído
        if (jogador == null)
        {
            GameObject jogadorObj = GameObject.FindWithTag("Wendell");
            if (jogadorObj != null)
            {
                jogador = jogadorObj.transform;
            }
            else
            {
                Debug.LogError("Nenhum objeto com a tag 'Jogador' foi encontrado na cena.", this);
            }
        }
    }

    void Update()
    {
        if (jogador == null) return;

        // Atualiza a posição da malha para seguir o jogador
        Vector3 novaPosicao = new Vector3(jogador.position.x, transform.position.y, jogador.position.z);
        transform.position = novaPosicao;

        // Atualiza a posição do jogador no shader
        if (materialDoOceano != null)
        {
            materialDoOceano.SetVector("_PlayerPosition", jogador.position);
        }
    }

    void GerarMalhaDoOceano()
    {
        // Criação da malha do oceano
        malhaOceano = new Mesh();
        GetComponent<MeshFilter>().mesh = malhaOceano;

        // Definindo os vertices, triangulos e UVs da malha
        vertices = new Vector3[(resolucao + 1) * (resolucao + 1)];
        triangulos = new int[resolucao * resolucao * 6];
        uvs = new Vector2[(resolucao + 1) * (resolucao + 1)];

        float metadeTamanho = tamanho / 2f;
        int indiceVertice = 0;
        int indiceTriangulo = 0;

        // Gerar os vertices e UVs
        for (int i = 0; i <= resolucao; i++)
        {
            for (int j = 0; j <= resolucao; j++)
            {
                float x = (i / (float)resolucao) * tamanho - metadeTamanho;
                float z = (j / (float)resolucao) * tamanho - metadeTamanho;
                vertices[indiceVertice] = new Vector3(x, 0, z);

                uvs[indiceVertice] = new Vector2(i / (float)resolucao, j / (float)resolucao);

                indiceVertice++;

                if (i < resolucao && j < resolucao)
                {
                    int atual = i * (resolucao + 1) + j;
                    int proximo = atual + resolucao + 1;

                    // Triângulo 1
                    triangulos[indiceTriangulo] = atual;
                    triangulos[indiceTriangulo + 1] = proximo;
                    triangulos[indiceTriangulo + 2] = atual + 1;

                    // Triângulo 2
                    triangulos[indiceTriangulo + 3] = atual + 1;
                    triangulos[indiceTriangulo + 4] = proximo;
                    triangulos[indiceTriangulo + 5] = proximo + 1;

                    indiceTriangulo += 6;
                }
            }
        }

        // Atribui vértices, triângulos e UVs à malha gerada
        malhaOceano.vertices = vertices;
        malhaOceano.triangles = triangulos;
        malhaOceano.uv = uvs;
        malhaOceano.RecalculateNormals();

        Debug.Log("Número de vértices gerados: " + malhaOceano.vertexCount);
    }
}
