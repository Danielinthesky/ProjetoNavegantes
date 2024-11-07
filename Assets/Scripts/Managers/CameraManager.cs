using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.InputSystem;

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

    private CameraJangada controles;

    void Awake()
    {
        controles = new CameraJangada();
    }

    void OnEnable()
    {
        controles.Touch.Touch.performed += OnTouchPerformed;
        controles.Touch.Touch.canceled += OnTouchCanceled;
        controles.Enable();

        botaoAlternarRecentramento.onClick.RemoveAllListeners();
        botaoAlternarRecentramento.onClick.AddListener(AlternarRecentramento);
        AtivarRecentramento();
    }

    void OnDisable()
    {
        controles.Touch.Touch.performed -= OnTouchPerformed;
        controles.Touch.Touch.canceled -= OnTouchCanceled;
        controles.Disable();
    }

    private void OnTouchPerformed(InputAction.CallbackContext context)
    {
        Touchscreen touchscreen = Touchscreen.current;
        if (touchscreen == null) return;

        var toque = touchscreen.primaryTouch;

        if (toque.press.isPressed)
        {
            if (!estaArrastando)
            {
                estaArrastando = true;
                ultimaPosicaoToque = toque.position.ReadValue();

                if (recentramentoAtivo)
                {
                    cameraFreeLook.m_XAxis.m_InputAxisName = "";
                    cameraFreeLook.m_YAxis.m_InputAxisName = "";
                }
            }
            else
            {
                Vector2 posicaoAtual = toque.position.ReadValue();
                Vector2 delta = posicaoAtual - ultimaPosicaoToque;
                ultimaPosicaoToque = posicaoAtual;

                if (!recentramentoAtivo)
                {
                    cameraFreeLook.m_XAxis.Value += delta.x * sensibilidadeDeslizeX;
                    cameraFreeLook.m_YAxis.Value -= delta.y * sensibilidadeDeslizeY;
                }
                else
                {
                    float entradaRotacao = -delta.x * sensibilidadeDeslizeX;
                    controladorJangada.AplicarRotacao(entradaRotacao);
                }
            }
        }
    }

    private void OnTouchCanceled(InputAction.CallbackContext context)
    {
        estaArrastando = false;

        if (recentramentoAtivo)
        {
            cameraFreeLook.m_XAxis.m_InputAxisName = "Mouse X";
            cameraFreeLook.m_YAxis.m_InputAxisName = "Mouse Y";
        }
    }

    public void AlternarRecentramento()
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
        cameraFreeLook.m_XAxis.m_InputAxisName = "Mouse X";
        cameraFreeLook.m_YAxis.m_InputAxisName = "Mouse Y";
        Debug.Log("Recentramento desativado");
    }

    private void AtivarRecentramento()
    {
        recentramentoAtivo = true;
        cameraFreeLook.m_RecenterToTargetHeading.m_WaitTime = 1.0f;
        cameraFreeLook.m_RecenterToTargetHeading.m_enabled = true;
        cameraFreeLook.m_XAxis.m_InputAxisName = "";
        cameraFreeLook.m_YAxis.m_InputAxisName = "";
        Debug.Log("Recentramento ativado");
    }
}
