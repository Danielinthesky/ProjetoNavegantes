using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using System.Collections;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject prefabPersonagem1;
    public GameObject prefabPersonagem2;
    public CinemachineFreeLook freeLookCamera; // Arraste a Cinemachine Free Look da cena para este campo
    private GameObject meuPersonagem;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Checa se já estamos na cena de jogo para instanciar o personagem diretamente
        if (SceneManager.GetActiveScene().name == "Ilha Tutorial")
        {
            InstanciarPersonagem();
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded chamado para a cena: " + scene.name);

        // Instancia o personagem se a cena carregada for a de multiplayer
        if (scene.name == "Ilha Tutorial")
        {
            InstanciarPersonagem();
        }
    }

    private void InstanciarPersonagem()
{
    GameObject[] pontosDeSpawn = GameObject.FindGameObjectsWithTag("SpawnPoint");

    if (pontosDeSpawn.Length == 0)
    {
        Debug.LogError("Nenhum ponto de spawn encontrado na cena!");
        return;
    }

    int indiceSpawn = PhotonNetwork.IsMasterClient ? 0 : 1;
    if (indiceSpawn >= pontosDeSpawn.Length)
    {
        indiceSpawn = Random.Range(0, pontosDeSpawn.Length);
    }

    Vector3 posicaoSpawn = pontosDeSpawn[indiceSpawn].transform.position;
    string nomePrefab = "Jogador1";

    Debug.Log($"Tentando instanciar o personagem {nomePrefab} no ponto de spawn {posicaoSpawn}");
    meuPersonagem = PhotonNetwork.Instantiate(nomePrefab, posicaoSpawn, Quaternion.identity);
    StartCoroutine(ForcarInicializacao(meuPersonagem));


    if (meuPersonagem == null)
    {
        Debug.LogError($"Falha ao instanciar o personagem {nomePrefab}");
        return;
    }

    Debug.Log($"Personagem instanciado com sucesso: {meuPersonagem.name}");

    // Verifique se os componentes estão presentes
    if (meuPersonagem.GetComponent<StaminaPersonagemMultiplayer>() == null)
    {
        Debug.LogError("StaminaPersonagemMultiplayer ausente no jogador instanciado!", meuPersonagem);
    }

    if (meuPersonagem.GetComponent<PhotonView>() == null)
    {
        Debug.LogError("PhotonView ausente no jogador instanciado!", meuPersonagem);
    }

    // Configuração da câmera
    if (freeLookCamera != null && meuPersonagem.GetComponent<PhotonView>().IsMine)
    {
        freeLookCamera.Follow = meuPersonagem.transform;
        freeLookCamera.LookAt = meuPersonagem.transform;
    }
    else if (freeLookCamera == null)
    {
        Debug.LogError("Cinemachine Free Look não foi atribuída no GameManager!");
    }
}

private IEnumerator ForcarInicializacao(GameObject personagem)
{
    yield return null; // Aguarda um frame para garantir que todos os componentes sejam inicializados

    var stamina = personagem.GetComponent<StaminaPersonagemMultiplayer>();
    if (stamina == null)
    {
        Debug.LogError("StaminaPersonagemMultiplayer ainda não encontrado após inicialização!", personagem);
    }
    else
    {
        Debug.Log("StaminaPersonagemMultiplayer inicializado com sucesso.", personagem);
    }
}

}
