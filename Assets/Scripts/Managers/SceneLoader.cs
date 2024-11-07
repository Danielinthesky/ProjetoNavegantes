using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Método para carregar uma cena pelo nome
    public void CarregarCenaPorNome(string nomeCena)
    {
        SceneManager.LoadScene("Cena de Carregamento");
    }

    // Método para carregar uma cena pelo índice (opcional)
    public void CarregarCenaPorIndice(int indiceCena)
    {
        SceneManager.LoadScene(indiceCena);
    }
}
