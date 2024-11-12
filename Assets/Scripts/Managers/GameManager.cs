using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

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
        if (SceneManager.GetActiveScene().name == "MultiplayerTeste")
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
        if (scene.name == "MultiplayerTeste")
        {
            InstanciarPersonagem();
        }
    }

    private void InstanciarPersonagem()
    {
        Vector3 posicaoSpawn = PhotonNetwork.IsMasterClient ? new Vector3(-2, 0, 0) : new Vector3(2, 0, 0);
        string nomePrefab = PhotonNetwork.IsMasterClient ? "Jogador1" : "Jogador2";

        Debug.Log("Tentando instanciar o personagem: " + nomePrefab);
        meuPersonagem = PhotonNetwork.Instantiate(nomePrefab, posicaoSpawn, Quaternion.identity);

        if (meuPersonagem != null)
        {
            Debug.Log("Personagem instanciado com sucesso: " + nomePrefab);

            // Verifique se o personagem instanciado pertence ao jogador local antes de configurar a câmera
            PhotonView photonView = meuPersonagem.GetComponent<PhotonView>();
            if (photonView != null && photonView.IsMine)
            {
                if (freeLookCamera != null)
                {
                    freeLookCamera.Follow = meuPersonagem.transform;
                    freeLookCamera.LookAt = meuPersonagem.transform;
                }
                else
                {
                    Debug.LogError("Cinemachine Free Look não foi atribuído no GameManager!");
                }
            }
        }
        else
        {
            Debug.LogError("Falha ao instanciar o personagem: " + nomePrefab);
        }
    }
}
