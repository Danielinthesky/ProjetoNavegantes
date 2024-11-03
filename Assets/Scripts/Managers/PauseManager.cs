using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private bool jogoPausado = false;

    public void TogglePause()
    {
        jogoPausado = !jogoPausado;

        if (jogoPausado)
        {
            Time.timeScale = 0;  // Pausa o jogo
            Debug.Log("Jogo pausado");
        }
        else
        {
            Time.timeScale = 1;  // Retoma o jogo
            Debug.Log("Jogo retomado");
        }
    }
}
