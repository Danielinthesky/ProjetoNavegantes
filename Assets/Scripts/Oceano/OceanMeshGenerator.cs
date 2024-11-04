using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class OceanMeshGenerator : MonoBehaviour {
    public int resolucao = 50; 
    public float tamanho = 500f; 
    public Material materialDoOceano; 

    private Mesh malhaOceano;
    private Vector3[] vertices;
    private int[] triangulos;
    private Vector2[] uvs; 

    public Transform jogador; 

    void Start() {
        
        GerarMalhaDoOceano();
        
        if (materialDoOceano != null) {
            GetComponent<MeshRenderer>().material = materialDoOceano;
        } else {
            Debug.LogError("Material de oceano não atribuído!");
        }
    }

    void Update() {
        // Atualiza a posiçao da malha para seguir o jogador
        Vector3 novaPosicao = new Vector3(jogador.position.x, transform.position.y, jogador.position.z);
        transform.position = novaPosicao;

        // Atualiza a posição do jogador no shader
        if (materialDoOceano != null && jogador != null) {
            materialDoOceano.SetVector("_PlayerPosition", jogador.position);
        }
    }

    void GerarMalhaDoOceano() {
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
        for (int i = 0; i <= resolucao; i++) {
            for (int j = 0; j <= resolucao; j++) {
                float x = (i / (float)resolucao) * tamanho - metadeTamanho;
                float z = (j / (float)resolucao) * tamanho - metadeTamanho;
                vertices[indiceVertice] = new Vector3(x, 0, z);

                
                uvs[indiceVertice] = new Vector2(i / (float)resolucao, j / (float)resolucao);
                
                indiceVertice++;

               
                if (i < resolucao && j < resolucao) {
                    int atual = i * (resolucao + 1) + j;
                    int proximo = atual + resolucao + 1;

                    // Triangulo 1
                    triangulos[indiceTriangulo] = atual;
                    triangulos[indiceTriangulo + 1] = proximo;
                    triangulos[indiceTriangulo + 2] = atual + 1;

                    // Triangulo 2
                    triangulos[indiceTriangulo + 3] = atual + 1;
                    triangulos[indiceTriangulo + 4] = proximo;
                    triangulos[indiceTriangulo + 5] = proximo + 1;

                    indiceTriangulo += 6;
                }
            }
        }

        // Atribui vertices, triangulos e UVs a malha gerada
        malhaOceano.vertices = vertices;
        malhaOceano.triangles = triangulos;
        malhaOceano.uv = uvs; // Atribuir UVs
        malhaOceano.RecalculateNormals();

        // Iimprime o numero de vertices no console
        Debug.Log("Número de vértices gerados: " + malhaOceano.vertexCount);
    }
}
