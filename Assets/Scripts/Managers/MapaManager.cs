using UnityEngine;
using UnityEngine.UI;

public class MapToggle : MonoBehaviour
{
    [SerializeField]public Camera cameraMapa;      // Referência à câmera do mapa
    public GameObject panelMapa;   // Referência ao Panel do mapa (UI)

    private bool mapaAtivo = false;

    void Start()
    {
        // Certifique-se de que o mapa está desativado inicialmente
        if (cameraMapa != null) cameraMapa.gameObject.SetActive(false);
        if (panelMapa != null) panelMapa.SetActive(false);
    }

    public void ToggleMapa()
    {
        mapaAtivo = !mapaAtivo;

        // Ativa/desativa a câmera e o Panel do mapa
        if (cameraMapa != null) cameraMapa.gameObject.SetActive(mapaAtivo);
        if (panelMapa != null) panelMapa.SetActive(mapaAtivo);
    }
}
