using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public CinemachineFreeLook cameraFreeLook;    
    public Button botaoAlternarRecentramento;     
    public float sensibilidadeDeslizeX = 0.2f;
    public float sensibilidadeDeslizeY = 0.2f;
    public RaftController controladorJangada;     

    private bool recentramentoAtivo = true;       
    private Vector2 ultimaPosicaoToque;
    private bool estaArrastando = false;

    void Start()
    {
        botaoAlternarRecentramento.onClick.RemoveAllListeners();
        botaoAlternarRecentramento.onClick.AddListener(AlternarRecentramento);
        AtivarRecentramento(); 
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch toque = Input.GetTouch(0);

            if (toque.phase == TouchPhase.Began)
            {
                estaArrastando = true;
                ultimaPosicaoToque = toque.position;

                // Bloqueia a rotaçao da camera para ambos os eixos se o recentramento estiver ativo
                if (recentramentoAtivo)
                {
                    // Desativa o input do eixo X e Y
                    cameraFreeLook.m_XAxis.m_InputAxisName = ""; 
                    cameraFreeLook.m_YAxis.m_InputAxisName = ""; 
                }
            }
            else if (toque.phase == TouchPhase.Moved && estaArrastando)
            {
                Vector2 delta = toque.position - ultimaPosicaoToque;
                ultimaPosicaoToque = toque.position;

                if (!recentramentoAtivo)
                {
                    // Controle de rotação da câmera em órbita em torno da jangada
                    cameraFreeLook.m_XAxis.Value += delta.x * sensibilidadeDeslizeX;
                    cameraFreeLook.m_YAxis.Value -= delta.y * sensibilidadeDeslizeY;
                }
                else
                {
                    // Controle de rotação da jangada ao longo do eixo Y
                    float entradaRotacao = -delta.x * sensibilidadeDeslizeX;
                    controladorJangada.AplicarRotacao(entradaRotacao);
                }
            }
            else if (toque.phase == TouchPhase.Ended || toque.phase == TouchPhase.Canceled)
            {
                estaArrastando = false;

                // Reativa o input do eixo X e Y após o toque, conforme o estado do botão
                if (recentramentoAtivo)
                {
                    cameraFreeLook.m_XAxis.m_InputAxisName = "Mouse X";  // Reativa o input do eixo X
                    cameraFreeLook.m_YAxis.m_InputAxisName = "Mouse Y";  // Reativa o input do eixo Y
                }
            }
        }
    }

    private void AlternarRecentramento()
    {
        if (recentramentoAtivo)
        {
            DesativarRecentramento();
        }
        else
        {
            AtivarRecentramento();
        }
    }

    private void DesativarRecentramento()
    {
        recentramentoAtivo = false;
        cameraFreeLook.m_RecenterToTargetHeading.m_enabled = false;
        Debug.Log("Recentramento desativado");
    }

    private void AtivarRecentramento()
    {
        recentramentoAtivo = true;
        cameraFreeLook.m_RecenterToTargetHeading.m_WaitTime = 1.0f; // Define o tempo de espera para 1 segundo
        cameraFreeLook.m_RecenterToTargetHeading.m_enabled = true;
        Debug.Log("Recentramento ativado");
    }
}
