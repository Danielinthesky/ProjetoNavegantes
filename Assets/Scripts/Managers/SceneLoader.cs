using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Método para carregar uma cena pelo nome
    public void CarregarCeenaSingleplayer(string nomeCena)
    {
        SceneManager.LoadScene("Jangada no Oceano");
    }

     public void CarregarCenaSalaMultiplayer(string nomeCena)
    {
        SceneManager.LoadScene("SalaMultiplayer");
    }

    // Método para carregar uma cena pelo índice (opcional)
    public void CarregarCenaPorIndice(int indiceCena)
    {
        SceneManager.LoadScene(indiceCena);
    }
}
