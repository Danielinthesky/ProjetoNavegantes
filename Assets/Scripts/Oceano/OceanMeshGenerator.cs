using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class OceanMeshGenerator : MonoBehaviour {
    public int resolution = 50; // Número de subdivisões (mais subdivisões = mais detalhado)
    public float size = 500f; // Tamanho total do oceano (área de interesse ao redor do jogador)
    public Material oceanMaterial; // O material que contém o seu Shader de água

    private Mesh oceanMesh;
    private Vector3[] vertices;
    private int[] triangles;
    private Vector2[] uvs; // Array de UVs

    public Transform player; // Referência ao jogador para centralizar a malha

    void Start() {
        // Inicializa a malha e define o material do oceano
        GenerateOceanMesh();
        
        if (oceanMaterial != null) {
            GetComponent<MeshRenderer>().material = oceanMaterial;
        } else {
            Debug.LogError("Material de oceano não atribuído!");
        }
    }

    void Update() {
        // Atualiza a posição da malha para seguir o jogador
        Vector3 newPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.position = newPosition;

        // Atualiza a posição do jogador no shader
        if (oceanMaterial != null && player != null) {
            oceanMaterial.SetVector("_PlayerPosition", player.position);
        }
    }

    void GenerateOceanMesh() {
        // Criação da malha do oceano
        oceanMesh = new Mesh();
        GetComponent<MeshFilter>().mesh = oceanMesh;

        // Definindo os vértices, triângulos e UVs da malha
        vertices = new Vector3[(resolution + 1) * (resolution + 1)];
        triangles = new int[resolution * resolution * 6];
        uvs = new Vector2[(resolution + 1) * (resolution + 1)];

        float halfSize = size / 2f;
        int vertIndex = 0;
        int triIndex = 0;

        // Gerar os vértices e UVs
        for (int i = 0; i <= resolution; i++) {
            for (int j = 0; j <= resolution; j++) {
                float x = (i / (float)resolution) * size - halfSize;
                float z = (j / (float)resolution) * size - halfSize;
                vertices[vertIndex] = new Vector3(x, 0, z);

                // Configurar UVs para que cubram a área de 0 a 1
                uvs[vertIndex] = new Vector2(i / (float)resolution, j / (float)resolution);
                
                vertIndex++;

                // Criar os triângulos (apenas se não estivermos na última linha ou coluna)
                if (i < resolution && j < resolution) {
                    int current = i * (resolution + 1) + j;
                    int next = current + resolution + 1;

                    // Triângulo 1
                    triangles[triIndex] = current;
                    triangles[triIndex + 1] = next;
                    triangles[triIndex + 2] = current + 1;

                    // Triângulo 2
                    triangles[triIndex + 3] = current + 1;
                    triangles[triIndex + 4] = next;
                    triangles[triIndex + 5] = next + 1;

                    triIndex += 6;
                }
            }
        }

        // Atribuir vértices, triângulos e UVs à malha
        oceanMesh.vertices = vertices;
        oceanMesh.triangles = triangles;
        oceanMesh.uv = uvs; // Atribuir UVs
        oceanMesh.RecalculateNormals();

        // Verificação: imprima o número de vértices no console
        Debug.Log("Número de vértices gerados: " + oceanMesh.vertexCount);
    }
}
