using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public CinemachineFreeLook freeLookCamera;    // Referência à câmera Cinemachine FreeLook
    public Button toggleRecenteringButton;        // Botão para ativar/desativar o recentramento
    public float swipeSensitivityX = 0.2f;
    public float swipeSensitivityY = 0.2f;
    public RaftController raftController;         // Referência ao controlador da jangada

    private bool isRecenteringEnabled = true;     // Estado atual do recentramento
    private Vector2 lastTouchPosition;
    private bool isDragging = false;

    void Start()
    {
        toggleRecenteringButton.onClick.RemoveAllListeners();
        toggleRecenteringButton.onClick.AddListener(ToggleRecentering);
        EnableRecentering();  // Inicializa com o recentramento ativado
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                isDragging = true;
                lastTouchPosition = touch.position;

                // Bloqueia a rotação da câmera para ambos os eixos se o recentramento estiver ativo
                if (isRecenteringEnabled)
                {
                    freeLookCamera.m_XAxis.m_InputAxisName = ""; // Desativa o input do eixo X
                    freeLookCamera.m_YAxis.m_InputAxisName = ""; // Desativa o input do eixo Y
                }
            }
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                Vector2 delta = touch.position - lastTouchPosition;
                lastTouchPosition = touch.position;

                if (!isRecenteringEnabled)
                {
                    // Controle de rotação da câmera em órbita em torno da jangada
                    freeLookCamera.m_XAxis.Value += delta.x * swipeSensitivityX;
                    freeLookCamera.m_YAxis.Value -= delta.y * swipeSensitivityY;
                }
                else
                {
                    // Controle de rotação da jangada ao longo do eixo Y
                    float rotacaoInput = -delta.x * swipeSensitivityX;
                    raftController.AplicarRotacao(rotacaoInput);
                }
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isDragging = false;

                // Reativa o input do eixo X e Y após o toque, conforme o estado do botão
                if (isRecenteringEnabled)
                {
                    freeLookCamera.m_XAxis.m_InputAxisName = "Mouse X";  // Reativa o input do eixo X
                    freeLookCamera.m_YAxis.m_InputAxisName = "Mouse Y";  // Reativa o input do eixo Y
                }
            }
        }
    }

    private void ToggleRecentering()
    {
        if (isRecenteringEnabled)
        {
            DisableRecentering();
        }
        else
        {
            EnableRecentering();
        }
    }

    private void DisableRecentering()
    {
        isRecenteringEnabled = false;
        freeLookCamera.m_RecenterToTargetHeading.m_enabled = false;
        Debug.Log("Recentering desativado");
    }

    private void EnableRecentering()
    {
        isRecenteringEnabled = true;
        freeLookCamera.m_RecenterToTargetHeading.m_WaitTime = 1.0f; // Define o tempo de espera para 1 segundo
        freeLookCamera.m_RecenterToTargetHeading.m_enabled = true;
        Debug.Log("Recentering ativado");
    }
}
