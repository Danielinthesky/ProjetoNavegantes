using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DetectorIlha : MonoBehaviour
{
    public GameObject uiEmbarcar; // Referência para o painel de UI de embarque
    public string nomeCenaIlha; // Nome da cena da ilha
    public RaftController raftController; // Referência ao script de movimento da jangada
    private float velocidadeOriginal; // Armazena a velocidade original da jangada

    void Start()
    {
        uiEmbarcar.SetActive(false); // Começa com o painel de UI desativado
        velocidadeOriginal = raftController.velocidadeMaxima; // Guarda a velocidade original da jangada
    }

    void OnTriggerEnter(Collider outro)
    {
        if (outro.CompareTag("Jangada")) // Verifica se o objeto entrando no trigger é a jangada
        {
            uiEmbarcar.SetActive(true); // Ativa o aviso de embarque
            raftController.velocidadeMaxima = 0f; // Pausa a jangada
        }
    }

    void OnTriggerExit(Collider outro)
    {
        if (outro.CompareTag("Jangada")) // Verifica se a jangada está saindo do trigger
        {
            uiEmbarcar.SetActive(false); // Desativa o aviso de embarque
            raftController.velocidadeMaxima = velocidadeOriginal; // Retorna a velocidade original
        }
    }

    public void Desembarcar()
    {
        SceneManager.LoadScene(nomeCenaIlha); // Carrega a cena da ilha
    }

    public void ContinuarNavegando()
    {
        uiEmbarcar.SetActive(false); // Desativa o aviso de embarque
        raftController.velocidadeMaxima = velocidadeOriginal; // Retorna a velocidade original
    }
}
